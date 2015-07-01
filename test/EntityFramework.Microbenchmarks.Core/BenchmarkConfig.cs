using Microsoft.Framework.Configuration;
using System;

namespace EntityFramework.Microbenchmarks.Core
{
    public class BenchmarkConfig
    {
        private static Lazy<BenchmarkConfig> _instance = new Lazy<BenchmarkConfig>(() =>
        {
            var config = new ConfigurationBuilder(".")
                .AddJsonFile("config.json")
                .AddEnvironmentVariables()
                .Build();

            return new BenchmarkConfig
            {
                RunIterations = bool.Parse(config.Get("benchmarks:runIterations")),
                ResultsDatabase = config.Get("benchmarks:resultDatabase"),
                SqlServer = config.Get("benchmarks:sqlServer")
            };
        });

        private BenchmarkConfig()
        {

        }

        public static BenchmarkConfig Instance
        {
            get { return _instance.Value; }
        }

        public bool RunIterations { get; private set; }

        public string ResultsDatabase { get; private set; }

        public string SqlServer { get; private set; }
    }
}
