using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Order;
using BulkRename.Class;

namespace  BulkRename.Benchmark.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ClassBenchmarks
    {
        public int a = 2;
        public int b = 3;

        public static Class1 testClass = new Class1();

        [Benchmark]
        public void AddBenchmark()
        {
            testClass.Add(a, b);
        }
    }
}