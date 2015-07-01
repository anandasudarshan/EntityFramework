using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace EntityFramework.Microbenchmarks.Core
{
    [XunitTestCaseDiscoverer("EntityFramework.Microbenchmarks.Core.BenchmarkTestCaseDiscoverer", "EntityFramework.Microbenchmarks.Core")]
    public class BenchmarkAttribute : FactAttribute
    {
        public int Iterations { get; set; } = 1;
        public int WarmupIterations { get; set; } 
    }

    public class BenchmarkVariation : DataAttribute
    {
        public BenchmarkVariation(string variationName, params object[] data)
        {
            VariationName = variationName;
            Data = data;
        }

        public string VariationName { get; private set; }

        public IEnumerable<object> Data { get; private set; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            throw new NotImplementedException();
        }
    }
}
