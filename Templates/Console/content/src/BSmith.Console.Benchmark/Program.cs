using System;
using BenchmarkDotNet.Running;
using BSmith.Console.Benchmark.Benchmarks;

namespace BSmith.Console.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ClassBenchmarks>();
        }
    }
}
