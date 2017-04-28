using Xunit;
using System.Threading.Tasks;
using Nomnio.TableCopyLibrary;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System;

namespace TableCopyTests
{


    public class TableCopyTests
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const string sourceConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
        const string targetConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
        const int batchSize = 100;

        [Theory]
        [InlineData(sourceConnectionString, targetConnectionString, batchSize)]
        public async Task CopyAsync_Should_Copy_All_Entities_Count(string _sourceConnectionString, string _targetConnectionString, int _batchSize)
        {
            var emptySourceTableName = RandomString(16);
            var targetTableName = RandomString(16);

            var emptySourceStorageAccount = GetCloudStorageAccount(_sourceConnectionString);
            var emptySourceTable = await GetTableAsync(emptySourceStorageAccount, emptySourceTableName);
            

            var targetStorageAccount = GetCloudStorageAccount(_targetConnectionString);
            var targetTable = await GetTableAsync(targetStorageAccount, targetTableName);


            //get enteties that we will fill source table with
            var fakeResults = PrepareTestData();
            await WriteToTableAsync(emptySourceTable,fakeResults);
            //copy our source table into target table
            var tableCopy = new DynamicTableCopy();
            await tableCopy.CopyAsync(_sourceConnectionString, emptySourceTableName, _targetConnectionString, targetTableName, _batchSize);
            
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

            var emptySourceStorageAccount = GetCloudStorageAccount(_sourceConnectionString);
            var emptySourceTable = await GetTableAsync(emptySourceStorageAccount, emptySourceTableName);

            var targetStorageAccount = GetCloudStorageAccount(_targetConnectionString);
            var targetTable = await GetTableAsync(targetStorageAccount, targetTableName);

            //get enteties that we will fill source table with
            var fakeResults = PrepareTestData();
            await WriteToTableAsync(emptySourceTable, fakeResults);

            //copy our source table into target table
            var tableCopy = new DynamicTableCopy();
            await tableCopy.CopyAsync(_sourceConnectionString, emptySourceTableName, _targetConnectionString, targetTableName, _batchSize);

            //get Table enteties that we copied to check if they were copied
            IEnumerable<CityWeatherTableEntity> expectedResults = await GetTableEntitiesAsync(emptySourceTable);
            IEnumerable<CityWeatherTableEntity> realResults = await GetTableEntitiesAsync(targetTable);

            var t = CheckIfSame(expectedResults, realResults);
            await DeleteTableAsync(emptySourceTable);
            await DeleteTableAsync(targetTable);

            Assert.True(t);
        }

        

        private string RandomString(int length)
        {
            var random = new Random();
            string tableName = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return "t" + tableName;
        }

        private async Task DeleteTableAsync(CloudTable table)
        {
            await table.DeleteIfExistsAsync();
        }

        private async Task WriteToTableAsync(CloudTable table, IEnumerable<CityWeatherTableEntity> enteties)
        {
            foreach (var item in enteties)
            {
                TableOperation insert = TableOperation.InsertOrReplace(item);
                await table.ExecuteAsync(insert);
            }
        }

        private async Task<IEnumerable<CityWeatherTableEntity>> GetTableEntitiesAsync(CloudTable table)
        {
            var tableQuery = new TableQuery<CityWeatherTableEntity>();

            IEnumerable<CityWeatherTableEntity> obj = new List<CityWeatherTableEntity>();
            TableContinuationToken continuationToken = null;
            do
            {
                // Retrieve a segment (up to 100 entities).
                var tableQueryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                continuationToken = tableQueryResult.ContinuationToken;

                obj = tableQueryResult.Results;

            } while (continuationToken != null);
            return obj;
        }

        private CloudStorageAccount GetCloudStorageAccount(string connectionString)
        {
            return CloudStorageAccount.Parse(connectionString);
        }

        private async Task<CloudTable> GetTableAsync(CloudStorageAccount storageAccount, string tableName)
        {
            var sourceTableClient = storageAccount.CreateCloudTableClient();
            var table = sourceTableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        private bool CheckIfSame(IEnumerable<CityWeatherTableEntity> a, IEnumerable<CityWeatherTableEntity> b)
        {
            int countA = a.Count();
            int countB = b.Count();

            for (int i = 0; i < countA; i++)
            {
                if (a.ElementAt(i).PartitionKey != b.ElementAt(i).PartitionKey && a.ElementAt(i).RowKey != b.ElementAt(i).RowKey && a.ElementAt(i).Temp != b.ElementAt(i).Temp && a.ElementAt(i).WeatherDescription != b.ElementAt(i).WeatherDescription && a.ElementAt(i).WeatherParameter != b.ElementAt(i).WeatherParameter)
                {
                    return false;
                }
            }
            return true;
        }

        private IEnumerable<CityWeatherTableEntity> PrepareTestData()
        {
            var testData = new List<CityWeatherTableEntity>();
            var random = new Random();
            int size = random.Next(200, 300);
            for (int i = 0; i < size; i++)
            {
                string cityName = new string(Enumerable.Repeat(chars, random.Next(1, 10))
              .Select(s => s[random.Next(s.Length)]).ToArray());
                string countryName = new string(Enumerable.Repeat(chars, random.Next(1, 3))
              .Select(s => s[random.Next(s.Length)]).ToArray());
                string weatherParameter = new string(Enumerable.Repeat(chars, random.Next(1, 15))
              .Select(s => s[random.Next(s.Length)]).ToArray());
                string weatherDescription = new string(Enumerable.Repeat(chars, random.Next(1, 16))
              .Select(s => s[random.Next(s.Length)]).ToArray());
                float temp = (float)random.NextDouble();
                testData.Add(new CityWeatherTableEntity(cityName, countryName, weatherParameter, weatherDescription, temp));
            }

            return testData;
        }
    }
}
