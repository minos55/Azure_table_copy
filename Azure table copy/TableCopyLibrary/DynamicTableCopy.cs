using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Nomnio.TableCopyLibrary.Interfaces;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace Nomnio.TableCopyLibrary
{
    public class DynamicTableCopy : ITableCopyService
    {
        ILogger myLog;
        public DynamicTableCopy()
        {
            myLog = Log.ForContext<DynamicTableCopy>();
        }

        public async Task CopyAsync(string _sourceConnectionString, string _sourceTableName, string _targetConnectionString, string _targetTableName, int _batchSize)
        {
            var sourceStorageAccount = GetCloudStorageAccount(_sourceConnectionString);
            var targetStorageAccount = GetCloudStorageAccount(_targetConnectionString); ;

            var sourceTable = await GetTableAsync(sourceStorageAccount, _sourceTableName);

            var targetTable = await GetTableAsync(targetStorageAccount, _targetTableName);

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

                        myLog.Information("Entity with the PartitionKey = ({PartitionKeyString}) and RowKey = ({RowKeyString}) copied from {Source} into table {Target}.", item.PartitionKey, item.RowKey, _sourceTableName, _targetTableName);
                    }
                    await targetTable.ExecuteBatchAsync(batchOperation);
                }
                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);
        }

        private CloudStorageAccount GetCloudStorageAccount(string connectionString)
        {
            CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(connectionString);
            myLog.Information("Connected to {Connection}", StorageAccount);
            return StorageAccount;
        }
        private async Task<CloudTable> GetTableAsync(CloudStorageAccount storageAccount, string tableName)
        {
            var sourceTableClient = storageAccount.CreateCloudTableClient();
            var table = sourceTableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}
