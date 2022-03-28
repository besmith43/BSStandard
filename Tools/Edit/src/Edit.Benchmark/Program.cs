using System;
using BenchmarkDotNet.Running;
using Edit.Benchmark.Benchmarks;

namespace Edit.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ClassBenchmarks>();
        }
    }
}
