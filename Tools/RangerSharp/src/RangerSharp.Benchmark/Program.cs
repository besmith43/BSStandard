using System;
using BenchmarkDotNet.Running;
using RangerSharp.Benchmark.Benchmarks;

namespace RangerSharp.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ClassBenchmarks>();
        }
    }
}
