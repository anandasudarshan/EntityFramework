using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace EntityFramework.Microbenchmarks.Core
{
    public class BenchmarkTestCase : XunitTestCase
    {
        public BenchmarkTestCase(
            int iterations,
            int warmupIterations,
            string variation,
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            ITestMethod testMethod,
            object[] testMethodArguments)
            : base(diagnosticMessageSink, defaultMethodDisplay, testMethod, new object[] { })
        {
            IEnumerable<object> args = new object[] { MetricCollector };

            if (testMethodArguments != null)
            {
                args = args.Concat(testMethodArguments);
            }

            TestMethodArguments = args.ToArray();

            Iterations = iterations;
            WarmupIterations = warmupIterations;
            Variation = variation;
        }

        public int Iterations { get; private set; }
        public int WarmupIterations { get; private set; }
        public string Variation { get; private set; }
        public MetricCollector MetricCollector { get; private set; } = new MetricCollector();

        public override Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            return new BenchmarkTestCaseRunner(this, DisplayName, SkipReason, constructorArguments, TestMethodArguments, messageBus, aggregator, cancellationTokenSource).RunAsync();
        }
    }

}
