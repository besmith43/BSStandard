using System;
using BenchmarkDotNet.Running;
using BSmith.PWSH.Benchmark.Benchmarks;

namespace BSmith.PWSH.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ClassBenchmarks>();
        }
    }
}
