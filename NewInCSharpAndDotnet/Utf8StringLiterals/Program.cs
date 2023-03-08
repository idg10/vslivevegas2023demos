using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using System.Text.Json;

BenchmarkRunner.Run<Utf8LiteralsBenchmarks>();

/// <summary>
/// Benchmark comparing property looking by string comparison, with NameEquals both with an ordinary
/// string and with a UTF-8 string constant.
/// </summary>
/// <remarks>
/// <para>Results:</para>
/// <code><![CDATA[
/// |                           Method |     Mean |    Error |   StdDev | Ratio | RatioSD |     Gen0 | Allocated | Alloc Ratio |
/// |--------------------------------- |---------:|---------:|---------:|------:|--------:|---------:|----------:|------------:|
/// |      ComparePropertyNameAsString | 15.70 ms | 0.293 ms | 0.274 ms |  1.00 |    0.00 | 906.2500 | 3920117 B |       1.000 |
/// |     PropertyNameEqualsWithString | 14.37 ms | 0.233 ms | 0.218 ms |  0.92 |    0.02 |        - |     103 B |       0.000 |
/// | PropertyNameEqualsWithUtf8String | 13.55 ms | 0.141 ms | 0.118 ms |  0.86 |    0.02 |        - |     103 B |       0.000 |
/// ]]></code>
/// </remarks>
[MemoryDiagnoser]
public class Utf8LiteralsBenchmarks
{
    private static readonly byte[] jsonUtf8;

    static Utf8LiteralsBenchmarks()
    {
        MemoryStream ms = new();
        using (Utf8JsonWriter jw = new(ms))
        {
            jw.WriteStartArray();
            for (int i = 0; i < 100_000; ++i)
            {
                jw.WriteStartObject();
                jw.WriteNumber($"prop{i % 100}", i);
                jw.WriteEndObject();
            }
            jw.WriteEndArray();
        }

        jsonUtf8 = ms.ToArray();
    }

    [Benchmark(Baseline = true)]
    public int ComparePropertyNameAsString()
    {
        int total = 0;
        using JsonDocument doc = JsonDocument.Parse(jsonUtf8);
        foreach (JsonElement elem in doc.RootElement.EnumerateArray())
        {
            foreach (JsonProperty prop in elem.EnumerateObject())
            {
                if (prop.Name == "prop42")
                {
                    total += 1;
                }
            }
        }

        return total;
    }

    [Benchmark]
    public int PropertyNameEqualsWithString()
    {
        int total = 0;
        using JsonDocument doc = JsonDocument.Parse(jsonUtf8);
        foreach (JsonElement elem in doc.RootElement.EnumerateArray())
        {
            foreach (JsonProperty prop in elem.EnumerateObject())
            {
                if (prop.NameEquals("prop42"))
                {
                    total += 1;
                }
            }
        }

        return total;
    }

    [Benchmark]
    public int PropertyNameEqualsWithUtf8String()
    {
        int total = 0;
        using JsonDocument doc = JsonDocument.Parse(jsonUtf8);
        foreach (JsonElement elem in doc.RootElement.EnumerateArray())
        {
            foreach (JsonProperty prop in elem.EnumerateObject())
            {
                if (prop.NameEquals("prop42"u8))
                {
                    total += 1;
                }
            }
        }

        return total;
    }
}