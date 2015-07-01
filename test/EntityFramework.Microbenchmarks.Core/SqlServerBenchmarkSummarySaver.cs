using Microsoft.Data.Entity;

namespace EntityFramework.Microbenchmarks.Core
{
    public class SqlServerBenchmarkSummarySaver : IBenchmarkSummarySaver
    {
        private readonly string _connectionString;

        public SqlServerBenchmarkSummarySaver(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SaveSummary(BenchmarkRunSummary summary)
        {
            using (var db = new BenchmarkContext(_connectionString))
            {
                db.Database.EnsureCreated();

                db.RunSummaries.Add(summary);
                db.SaveChanges();
            }
        }
    }

    public class BenchmarkContext : DbContext
    {
        private readonly string _connectionString;

        public BenchmarkContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<BenchmarkRunSummary> RunSummaries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BenchmarkRunSummary>()
                .ToSqlServerTable("Runs");

            modelBuilder.Entity<BenchmarkRunSummary>()
                .Property<int>("Id");

            modelBuilder.Entity<BenchmarkRunSummary>()
                .Key("Id");
        }
    }
}
