using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace EntityFramework.Microbenchmarks.Core
{
    public class BenchmarkTestCaseRunner : XunitTestCaseRunner
    {
        public BenchmarkTestCaseRunner(
            BenchmarkTestCase testCase,
            string displayName,
            string skipReason,
            object[] constructorArguments,
            object[] testMethodArguments,
            IMessageBus messageBus,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
            : base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource)
        {
            TestCase = testCase;
        }

        public new BenchmarkTestCase TestCase { get; private set; }

        protected override async Task<RunSummary> RunTestAsync()
        {
            var summary = new BenchmarkRunSummary
            {
                Test = DisplayName,
                Variation = TestCase.Variation,
                RunStarted = DateTime.UtcNow,
                MachineName = Environment.MachineName,
                ProductVersion = "7.0.0",
                Framework = "net46",
            };

            for (int i = 0; i < TestCase.WarmupIterations; i++)
            {
                var runner = CreateRunner(string.Format("warmup {0}/{1}", i + 1, TestCase.Iterations));
                summary.Aggregate(await runner.RunAsync());
            }

            for (int i = 0; i < TestCase.Iterations; i++)
            {
                var iterationSummary = new BenchmarkIterationSummary();
                var runner = CreateRunner(string.Format("iteration {0}/{1}", i + 1, TestCase.Iterations));
                iterationSummary.Aggregate(await runner.RunAsync());
                iterationSummary.TimeEllapsed = TestCase.MetricCollector.TimeEllapsed;
                iterationSummary.MemoryDelta = TestCase.MetricCollector.MemoryDelta;
                summary.Aggregate(iterationSummary);
            }


            summary.PopulateMetrics();

            if (BenchmarkConfig.Instance.ResultsDatabase != null)
            {
                new SqlServerBenchmarkSummarySaver(BenchmarkConfig.Instance.ResultsDatabase).SaveSummary(summary);
            }

            return summary;
        }

        private XunitTestRunner CreateRunner(string iterationPostfix)
        {
            var test = new XunitTest(TestCase, string.Format("{0} ({1})", DisplayName, iterationPostfix));
            return new XunitTestRunner(test, MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments, SkipReason, BeforeAfterAttributes, Aggregator, CancellationTokenSource);
        }
    }
}
