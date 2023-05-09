using BenchmarkDotNet.Running;
using System.Series.Tests;

namespace Undersoft.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            //  BenchmarkRunner.Run<MathsetBenchmark>();

            // var metod = new Catalog64Benchmark();

            //  metod.Dictionary_Add_Test();

            BenchmarkRunner.Run<Catalog64Benchmark>();

        }
    }
}
