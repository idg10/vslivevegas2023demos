// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using System.Collections.Frozen;
using System.Collections.Immutable;

BenchmarkRunner.Run<DictionaryBenchmarks>();

/// <summary>
/// Compare read performance of <see cref="Dictionary{TKey, TValue}"/>,
/// <see cref="FrozenDictionary{TKey, TValue}"/> and
/// <see cref="ImmutableDictionary{TKey, TValue}"/>.
/// </summary>
/// <remarks>
/// <para>Results:</para>
/// <code><![CDATA[
/// |    Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |
/// |---------- |----------:|----------:|----------:|----------:|------:|--------:|
/// |  Ordinary |  9.499 ms | 0.3950 ms | 1.1585 ms |  9.076 ms |  1.00 |    0.00 |
/// |    Frozen |  8.707 ms | 0.3332 ms | 0.9719 ms |  8.432 ms |  0.93 |    0.15 |
/// | Immutable | 46.399 ms | 1.5854 ms | 4.6747 ms | 45.085 ms |  4.96 |    0.80 |
/// ]]></code>
/// </remarks>
public class DictionaryBenchmarks
{
    private readonly string[] keys = new string[100_000];
    private readonly Dictionary<string, int> ordinaryDictionary = new();
    private readonly ImmutableDictionary<string, int> immutableDictionary;
    private readonly FrozenDictionary<string, int> frozenDictionary;

    private readonly Random r = new(0);

    public DictionaryBenchmarks()
    {
        for (int i = 0; i < keys.Length; ++i)
        {
            string key = i.ToString();
            keys[i] = key;
            ordinaryDictionary.Add(key, i);
        }
        immutableDictionary = ImmutableDictionary.CreateRange(ordinaryDictionary);
        frozenDictionary = ordinaryDictionary.ToFrozenDictionary();
    }

    [Benchmark(Baseline = true)]
    public int Ordinary()
    {
        int total = 0;
        for (int i = 0; i < 100_000; ++i)
        {
            total += ordinaryDictionary[keys[r.Next(keys.Length)]];
        }

        return total;
    }

    [Benchmark]
    public int Frozen()
    {
        int total = 0;
        for (int i = 0; i < 100_000; ++i)
        {
            total += frozenDictionary[keys[r.Next(keys.Length)]];
        }

        return total;
    }

    [Benchmark]
    public int Immutable()
    {
        int total = 0;
        for (int i = 0; i < 100_000; ++i)
        {
            total += immutableDictionary[keys[r.Next(keys.Length)]];
        }

        return total;
    }
}