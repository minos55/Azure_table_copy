using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TableCopyTests
{
    public abstract class TestBase
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        protected string RandomString(int length)
        {
            var random = new Random();
            string tableName = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
             return "t"+tableName;
        }

        protected async Task DeleteTableAsync(CloudTable table)
        {
            await table.DeleteIfExistsAsync();
        }

        protected async Task WriteToTableAsync(CloudTable table, IEnumerable<CityWeatherTableEntity> enteties)
        {
            foreach (var item in enteties)
            {
                TableOperation insert = TableOperation.InsertOrReplace(item);
                await table.ExecuteAsync(insert);
            }
        }

        protected async Task<IEnumerable<CityWeatherTableEntity>> GetTableEntitiesAsync(CloudTable table)
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

        protected CloudStorageAccount GetCloudStorageAccount(string connectionString)
        {
            CloudStorageAccount StorageAccount = null;
            CloudStorageAccount.TryParse(connectionString, out StorageAccount);

            return StorageAccount;
        }

        protected async Task<CloudTable> GetTableAsync(CloudStorageAccount storageAccount, string tableName)
        {
            var sourceTableClient = storageAccount.CreateCloudTableClient();
            var table = sourceTableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        protected bool CheckIfSame(IEnumerable<CityWeatherTableEntity> a, IEnumerable<CityWeatherTableEntity> b)
        {
            int countA = a.Count();
            int countB = b.Count();
            
            for(int i=0;i<countA;i++)
            {
                if(a.ElementAt(i).PartitionKey != b.ElementAt(i).PartitionKey&& a.ElementAt(i).RowKey != b.ElementAt(i).RowKey && a.ElementAt(i).Temp != b.ElementAt(i).Temp&& a.ElementAt(i).WeatherDescription != b.ElementAt(i).WeatherDescription&& a.ElementAt(i).WeatherParameter != b.ElementAt(i).WeatherParameter)
                {
                    return false;
                }
            }
            return true;
        }

        protected IEnumerable<CityWeatherTableEntity> PrepareTestData()
        {
            var testData = new List<CityWeatherTableEntity>();
            var random = new Random();
            int size = random.Next(200, 300);
            for(int i=0;i<size;i++)
            {
                string cityName = new string(Enumerable.Repeat(chars, random.Next(1,10))
              .Select(s => s[random.Next(s.Length)]).ToArray());
                string countryName = new string(Enumerable.Repeat(chars, random.Next(1, 3))
              .Select(s => s[random.Next(s.Length)]).ToArray());
                string weatherParameter = new string(Enumerable.Repeat(chars, random.Next(1, 15))
              .Select(s => s[random.Next(s.Length)]).ToArray());
                string weatherDescription = new string(Enumerable.Repeat(chars, random.Next(1, 16))
              .Select(s => s[random.Next(s.Length)]).ToArray());
                float temp = (float) random.NextDouble();
                testData.Add(new CityWeatherTableEntity(cityName, countryName, weatherParameter, weatherDescription, temp));
            }

            return testData;
        }
    }
}
