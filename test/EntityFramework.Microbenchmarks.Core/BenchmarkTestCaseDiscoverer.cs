using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EntityFramework.Microbenchmarks.Core
{
    public class BenchmarkTestCaseDiscoverer : IXunitTestCaseDiscoverer
    {
        readonly IMessageSink diagnosticMessageSink;

        public BenchmarkTestCaseDiscoverer(IMessageSink diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            var variations = testMethod.Method
                .GetCustomAttributes(typeof(BenchmarkVariation))
                .ToDictionary(
                    a => a.GetNamedArgument<string>(nameof(BenchmarkVariation.VariationName)),
                    a => a.GetNamedArgument<object[]>(nameof(BenchmarkVariation.Data)));

            if (!variations.Any())
            {
                variations.Add("default", new object[] { });
            }

            var tests = new List<IXunitTestCase>();
            foreach (var variation in variations)
            {
                if (BenchmarkConfig.Instance.RunIterations)
                {
                    tests.Add(new BenchmarkTestCase(
                        factAttribute.GetNamedArgument<int>(nameof(BenchmarkAttribute.Iterations)),
                        factAttribute.GetNamedArgument<int>(nameof(BenchmarkAttribute.WarmupIterations)),
                        variation.Key,
                        diagnosticMessageSink,
                        discoveryOptions.MethodDisplayOrDefault(),
                        testMethod,
                        variation.Value));
                }
                else
                {
                    var args = new object[] { new MetricCollector() }
                        .Concat(variation.Value)
                        .ToArray();

                    tests.Add(new XunitTestCase(diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, args));
                }
            }

            return tests;
        }
    }
}
