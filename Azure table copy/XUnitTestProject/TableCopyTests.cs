using Xunit;
using System.Threading.Tasks;
using Nomnio.TableCopyLibrary;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Auth;
using System;

namespace TableCopyTests
{


    public class TableCopyTests : TestBase
    {
        const string sourceConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
        const string sourceTableName = "weathertable";
        const string targetConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
        const int batchSize = 100;

        [Theory]
        [InlineData(sourceConnectionString, targetConnectionString, batchSize)]
        public async Task CopyAsync_Should_Copy_All_Entities_Count(string _sourceConnectionString, string _targetConnectionString, int _batchSize)
        {
            var emptySourceTableName = RandomString(16);
            var targetTableName = RandomString(16);

            var testStorageAccount = GetCloudStorageAccount(sourceConnectionString);
            var testTable = await GetTableAsync(testStorageAccount, sourceTableName);
            

            var emptySourceStorageAccount = GetCloudStorageAccount(_sourceConnectionString);
            var emptySourceTable = await GetTableAsync(emptySourceStorageAccount, emptySourceTableName);
            

            var targetStorageAccount = GetCloudStorageAccount(_targetConnectionString);
            var targetTable = await GetTableAsync(targetStorageAccount, targetTableName);
            

            //get enteties that we will fill source table with
            var fakeResults = await GetTableEntitiesAsync(testTable);
            await WriteToTableAsync(emptySourceTable,fakeResults);
            //copy our source table into target table
            var tableCopy = new TableCopy();
            var test = await tableCopy.CopyAsync(_sourceConnectionString, emptySourceTableName, _targetConnectionString, targetTableName, _batchSize);
            
            //get Table enteties that we copied to check if they were copied
            IEnumerable<CityWeatherTableEntity> realResults= await GetTableEntitiesAsync(targetTable);

            int expected = fakeResults.Count();
            int results = realResults.Count();

            await DeleteTableAsync(emptySourceTable);
            await DeleteTableAsync(targetTable);

            Assert.Equal(expected, results);
        }

        [Theory]
        [InlineData(sourceConnectionString, targetConnectionString, batchSize)]
        public async Task CopyAsync_Should_Copy_All_Entities_Check(string _sourceConnectionString, string _targetConnectionString, int _batchSize)
        {
            var emptySourceTableName = RandomString(16);
            var targetTableName = RandomString(16);

            var testStorageAccount = GetCloudStorageAccount(sourceConnectionString);
            var testTable = await GetTableAsync(testStorageAccount, sourceTableName);

            var emptySourceStorageAccount = GetCloudStorageAccount(_sourceConnectionString);
            var emptySourceTable = await GetTableAsync(emptySourceStorageAccount, emptySourceTableName);

            var targetStorageAccount = GetCloudStorageAccount(_targetConnectionString);
            var targetTable = await GetTableAsync(targetStorageAccount, targetTableName);

            //get enteties that we will fill source table with
            var fakeResults = await GetTableEntitiesAsync(testTable);
            await WriteToTableAsync(emptySourceTable, fakeResults);
            //copy our source table into target table
            var tableCopy = new TableCopy();
            var test = await tableCopy.CopyAsync(_sourceConnectionString, emptySourceTableName, _targetConnectionString, targetTableName, _batchSize);

            //get Table enteties that we copied to check if they were copied
            IEnumerable<CityWeatherTableEntity> realResults = await GetTableEntitiesAsync(targetTable);

            var t = CheckIfSame(fakeResults,realResults);
            await DeleteTableAsync(emptySourceTable);
            await DeleteTableAsync(targetTable);

            Assert.True(t);
        }
    }
}
