using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;

namespace EntityFramework.Microbenchmarks.Core
{
    public class BenchmarkRunSummary : RunSummary
    {
        private List<BenchmarkIterationSummary> _iterations = new List<BenchmarkIterationSummary>();

        // Dimensions
        public string Test { get; set; }
        public string Scenario { get; set; }
        public string Variation { get; set; }
        public string MachineName { get; set; }
        public string Framework { get; set; }
        public string ProductVersion { get; set; }
        public DateTime RunStarted { get; set; }

        // Metrics
        public long TimeElapsedAverage { get; private set; }
        public long TimeElapsedPercentile99 { get; private set; }
        public long TimeElapsedPercentile95 { get; private set; }
        public long TimeElapsedPercentile90 { get; private set; }
        public double TimeElapsedStandardDeviation { get; private set; }

        public long MemoryDeltaAverage { get; private set; }
        public long MemoryDeltaPercentile99 { get; private set; }
        public long MemoryDeltaPercentile95 { get; private set; }
        public long MemoryDeltaPercentile90 { get; private set; }
        public double MemoryDeltaStandardDeviation { get; private set; }

        public IEnumerable<BenchmarkIterationSummary> IterationSummaries
        {
            get { return _iterations; }
        }

        public void Aggregate(BenchmarkIterationSummary summary)
        {
            base.Aggregate(summary);
            _iterations.Add(summary);
        }

        public void PopulateMetrics()
        {
            var elapsedTimes = IterationSummaries.Select(i => i.TimeEllapsed).ToArray();
            TimeElapsedAverage = elapsedTimes.Sum() / elapsedTimes.Length;
            TimeElapsedStandardDeviation = StandardDeviation(elapsedTimes, TimeElapsedAverage);
            TimeElapsedPercentile99 = Percentile(elapsedTimes, 0.99);
            TimeElapsedPercentile95 = Percentile(elapsedTimes, 0.95);
            TimeElapsedPercentile90 = Percentile(elapsedTimes, 0.90);

            var memoryDeltas = IterationSummaries.Select(i => i.MemoryDelta).ToArray();
            MemoryDeltaAverage = memoryDeltas.Sum() / memoryDeltas.Length;
            MemoryDeltaPercentile99 = Percentile(memoryDeltas, 0.99);
            MemoryDeltaPercentile95 = Percentile(memoryDeltas, 0.95);
            MemoryDeltaPercentile90 = Percentile(memoryDeltas, 0.90);
            MemoryDeltaStandardDeviation = StandardDeviation(memoryDeltas, MemoryDeltaAverage);
        }

        private static long Percentile(IEnumerable<long> results, double percentile)
        {
            return results.OrderBy(r => r).ElementAt((int)(results.Count() * percentile));
        }

        private static double StandardDeviation(IEnumerable<long> results, long average)
        {
            return Math.Sqrt(results
                .Select(r => r - average)
                .Select(r => r * r)
                .Sum()
                / results.Count());
        }
    }
}
