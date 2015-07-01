namespace EntityFramework.Microbenchmarks.Core
{
    public interface IBenchmarkSummarySaver
    {
        void SaveSummary(BenchmarkRunSummary summary);
    }
}
