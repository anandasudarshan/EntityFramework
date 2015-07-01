using Xunit.Sdk;

namespace EntityFramework.Microbenchmarks.Core
{
    public class BenchmarkIterationSummary : RunSummary
    {
        public long TimeEllapsed { get; set; }
        public long MemoryDelta { get; set; }
    }
}
