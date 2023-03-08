using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<StringConstantPatternBenchmarks>();

/// <summary>
/// Test performance impact of span-based string pattern matching compared with
/// substrings.
/// </summary>
/// <remarks>
/// <para>Spoilers!</para>
/// <code><![CDATA[
/// |            Method |     Mean |   Error |  StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
/// | ----------------- |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
/// | MatchAsSubstring  | 142.3 ns | 2.89 ns | 2.70 ns |  1.00 |    0.00 | 0.0229 |      96 B |       1.00  |
/// | MatchAsSpan       | 109.9 ns | 1.55 ns | 1.45 ns |  0.77 |    0.02 |      - |         - |       0.00  |
/// ]]></code>
/// </remarks>
[MemoryDiagnoser]
public class StringConstantPatternBenchmarks
{
    private readonly static string[] Urls =
    {
        "http://example.com/foo",
        "https://example.com/foo",
        "ftp://example.com/foo",
    };

    [Benchmark(Baseline = true)]
    public int MatchAsSubstring()
    {
        int defeatOveroptimization = 0;

        foreach (string url in Urls)
        {
            int schemeEnd = url.IndexOf(":");
            string type = url[..schemeEnd] switch
            {
                "http" => "Hypertext Transfer Protocol",
                "https" => "Secure Hypertext Transfer Protocol",
                "ftp" => "File Transfer Protocol",
                _ => "Unknown"
            };
            defeatOveroptimization += type.Length;
        }

        return defeatOveroptimization;
    }

    [Benchmark]
    public int MatchAsSpan()
    {
        int defeatOveroptimization = 0;

        foreach (string url in Urls)
        {
            int schemeEnd = url.IndexOf(":");
            string type = url.AsSpan()[..schemeEnd] switch
            {
                "http" => "Hypertext Transfer Protocol",
                "https" => "Secure Hypertext Transfer Protocol",
                "ftp" => "File Transfer Protocol",
                _ => "Unknown"
            };
            defeatOveroptimization += type.Length;
        }

        return defeatOveroptimization;
    }
}