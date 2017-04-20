using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TableCopyLibrary
{
    public class TableCopy : ITableCopy
    {
        public TableCopy()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .WriteTo.RollingFile("log-{Date}.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

        }

        public async Task<bool> CopyAsync(string _sourceConnectionString, string _sourceTableName, string _targetConnectionString, string _targetTableName, int _batchSize)
        {
            try
            {
                var sourceStorageAccount =GetCloudStorageAccount(_sourceConnectionString);
                var targetStorageAccount = GetCloudStorageAccount(_targetConnectionString); ;

                var sourceTable = GetTable(sourceStorageAccount, _sourceTableName);
                await sourceTable.CreateIfNotExistsAsync();

                var targetTable = GetTable(targetStorageAccount, _targetTableName);
                await targetTable.CreateIfNotExistsAsync();


                var tableQuery = new TableQuery().Take(_batchSize);
                
                TableContinuationToken continuationToken = null;
                do
                {
                    // Retrieve a segment (up to 100 entities).
                    var tableQueryResult = await sourceTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                    // Assign the new continuation token to tell the service where to
                    // continue on the next iteration (or null if it has reached the end).
                    continuationToken = tableQueryResult.ContinuationToken;

                    var partitionList = tableQueryResult.Results.GroupBy(p => p.PartitionKey)
                        .Select(g => g.First().PartitionKey)
                        .ToList();

                    foreach (var partition in partitionList)
                    {
                        var batchOperation = new TableBatchOperation();

                        foreach (var item in tableQueryResult.Results.Where(x => x.PartitionKey == partition))
                        {
                            batchOperation.InsertOrReplace(item);

                            Log.Information($"Entity with the PartitionKey = ({item.PartitionKey}) and RowKey = ({item.RowKey}) copied from {_sourceTableName} into table {_targetTableName}.");
                        }
                        await targetTable.ExecuteBatchAsync(batchOperation);
                    }

                    // Loop until a null continuation token is received, indicating the end of the table.
                } while (continuationToken != null);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }

        private CloudStorageAccount GetCloudStorageAccount(string connectionString)
        {
            CloudStorageAccount StorageAccount = null;
            bool test = CloudStorageAccount.TryParse(connectionString, out StorageAccount);
            if (!test)
            {
                throw new Exception("Connection string is wrong.");
            }
            return StorageAccount;
        }
        private CloudTable GetTable(CloudStorageAccount storageAccount, string tableName)
        {
            var sourceTableClient = storageAccount.CreateCloudTableClient();
            Log.Information("Connected to {Connection}", storageAccount);

            return sourceTableClient.GetTableReference(tableName);
        }
    }
}
