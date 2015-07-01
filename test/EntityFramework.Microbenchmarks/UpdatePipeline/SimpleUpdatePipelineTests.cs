// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using EntityFramework.Microbenchmarks.Core;
using EntityFramework.Microbenchmarks.Core.Models.Orders;
using EntityFramework.Microbenchmarks.Models.Orders;
using Microsoft.Data.Entity;
using Xunit;

namespace EntityFramework.Microbenchmarks.UpdatePipeline
{
    public class SimpleUpdatePipelineTests
    {
        private static readonly string _connectionString 
            = $@"Server={BenchmarkConfig.Instance.SqlServer};Database=Perf_UpdatePipeline_Simple;Integrated Security=True;MultipleActiveResultSets=true;";

        public SimpleUpdatePipelineTests()
        {
            EnsureDatabaseSetup();
        }

        [Benchmark(Iterations = 100, WarmupIterations = 5)]
        [BenchmarkVariation("Batching Off", false)]
        [BenchmarkVariation("Batching On", true)]
        public void Insert(MetricCollector collector, bool disableBatching)
        {
            using (var context = new OrdersContext(_connectionString, disableBatching))
            {
                using (context.Database.BeginTransaction())
                {
                    for (var i = 0; i < 1000; i++)
                    {
                        context.Customers.Add(new Customer { Name = "New Customer " + i });
                    }

                    collector.StartCollection();
                    var records = context.SaveChanges();
                    collector.StopCollection();

                    Assert.Equal(1000, records);
                }
            }
        }

        [Benchmark(Iterations = 100, WarmupIterations = 5)]
        public void Update(MetricCollector collector)
        {
            Insert(collector, false);
        }

        [Benchmark(Iterations = 100, WarmupIterations = 5)]
        public void Update_WithoutBatching(MetricCollector collector)
        {
            Insert(collector, true);
        }

        private static void Update(MetricCollector collector, bool disableBatching)
        {
            using (var context = new OrdersContext(_connectionString, disableBatching))
            {
                using (context.Database.BeginTransaction())
                {
                    foreach (var customer in context.Customers)
                    {
                        customer.Name += " Modified";
                    }

                    collector.StartCollection();
                    var records = context.SaveChanges();
                    collector.StopCollection();

                    Assert.Equal(1000, records);
                }
            }
        }

        [Benchmark(Iterations = 100, WarmupIterations = 5)]
        public void Delete(MetricCollector collector)
        {
            Insert(collector, false);
        }

        [Benchmark(Iterations = 100, WarmupIterations = 5)]
        public void Delete_WithoutBatching(MetricCollector collector)
        {
            Insert(collector, true);
        }

        private static void Delete(MetricCollector collector, bool disableBatching)
        {
            using (var context = new OrdersContext(_connectionString, disableBatching))
            {
                using (context.Database.BeginTransaction())
                {
                    foreach (var customer in context.Customers)
                    {
                        context.Customers.Remove(customer);
                    }

                    collector.StartCollection();
                    var records = context.SaveChanges();
                    collector.StopCollection();

                    Assert.Equal(1000, records);
                }
            }
        }

        [Benchmark(Iterations = 100, WarmupIterations = 5)]
        public void Mixed(MetricCollector collector)
        {
            Insert(collector, false);
        }

        [Benchmark(Iterations = 100, WarmupIterations = 5)]
        public void Mixed_WithoutBatching(MetricCollector collector)
        {
            Insert(collector, true);
        }

        private static void Mixed(MetricCollector collector, bool disableBatching)
        {
            using (var context = new OrdersContext(_connectionString, disableBatching))
            {
                using (context.Database.BeginTransaction())
                {
                    var customers = context.Customers.ToArray();

                    for (var i = 0; i < 333; i++)
                    {
                        context.Customers.Add(new Customer { Name = "New Customer " + i });
                    }

                    for (var i = 0; i < 1000; i += 3)
                    {
                        context.Customers.Remove(customers[i]);
                    }

                    for (var i = 1; i < 1000; i += 3)
                    {
                        customers[i].Name += " Modified";
                    }

                    collector.StartCollection();
                    var records = context.SaveChanges();
                    collector.StopCollection();

                    Assert.Equal(1000, records);
                }
            }
        }

        private static void EnsureDatabaseSetup()
        {
            new OrdersSeedData().EnsureCreated(
                _connectionString,
                productCount: 0,
                customerCount: 1000,
                ordersPerCustomer: 0,
                linesPerOrder: 0);
        }
    }
}
