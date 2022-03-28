using System;
using BenchmarkDotNet.Running;
using BulkRename.Benchmark.Benchmarks;

namespace BulkRename.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ClassBenchmarks>();
        }
    }
}
