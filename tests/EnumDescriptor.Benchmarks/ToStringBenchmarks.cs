using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using EnumDescriptor.Core;

namespace EnumDescriptor.Benchmarks
{
    public class ToStringBenchmarks
    {
        [Benchmark]
        public string GeneratedToStringBenchmark()
        {
            return BenchmarkEnum.Value1.ToStringFast();
        }

        [Benchmark]
        public string OptimizedToStringBenchmark()
        {
            return Test.Value2.OptimizedToString();
        }

        [Benchmark]
        public string ToStringNaiveBenchmark()
        {
            return Test.Value2.ToStringNaive();
        }
    }
}