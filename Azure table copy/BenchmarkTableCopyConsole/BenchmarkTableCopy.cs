﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using Nomnio.TableCopyLibrary;
using System.Threading.Tasks;

namespace BenchmarkTableCopyConsole
{
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 5, targetCount: 5, id: "FastAndDirtyJob")]
    public class BenchmarkTableCopy
    {
        private DynamicTableCopy Copy;
        private string SourceConnectionString;
        private string SourceTableName;
        private string TargetConnectionString;
        private string TargetTableName;
        private int BatchSize;
        public BenchmarkTableCopy()
        {}

        [Setup]
        public void SetupData()
        {
            Copy = new DynamicTableCopy();

            SourceConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
            SourceTableName = "weathertable";
            TargetConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
            TargetTableName = "BenchWeatherTable";
            BatchSize = 1;
        }

        [Benchmark]
        public async Task BenchBatchSize1() => await Copy.CopyAsync(SourceConnectionString, SourceTableName, TargetConnectionString, TargetTableName + "1", BatchSize);

        [Benchmark]
        public async Task BenchBatchSize2() => await Copy.CopyAsync(SourceConnectionString, SourceTableName, TargetConnectionString, TargetTableName + "2", BatchSize*10);

        [Benchmark]
        public async Task BenchBatchSize3() => await Copy.CopyAsync(SourceConnectionString, SourceTableName, TargetConnectionString, TargetTableName + "3", BatchSize*100);
    }
}
