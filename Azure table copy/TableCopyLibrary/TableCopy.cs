using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Nomnio.TableCopyLibrary.Interfaces;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nomnio.TableCopyLibrary
{
    public class TableCopy : ITableCopy
    {
        ILogger myLog;
        public TableCopy()
        {
            myLog = Log.ForContext<TableCopy>();
        }

        public async Task CopyAsync(string _sourceConnectionString, string _sourceTableName, string _targetConnectionString, string _targetTableName, int _batchSize)
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

                            myLog.Information("Entity with the PartitionKey = ({PartitionKeyString}) and RowKey = ({RowKeyString}) copied from {Source} into table {Target}.", item.PartitionKey, item.RowKey, _sourceTableName, _targetTableName);
                        }
                        await targetTable.ExecuteBatchAsync(batchOperation);
                    }

                    // Loop until a null continuation token is received, indicating the end of the table.
                } while (continuationToken != null);
            }
            catch (Exception ex)
            {
                myLog.Error(ex.Message);
            }
        }

        private CloudStorageAccount GetCloudStorageAccount(string connectionString)
        {
            CloudStorageAccount StorageAccount = null;
            bool test = CloudStorageAccount.TryParse(connectionString, out StorageAccount);
            if (!test)
            {
                throw new Exception("Connection string is wrong.");
            }
            else
            {
                myLog.Information("Connected to {Connection}", StorageAccount);
            }
            return StorageAccount;
        }
        private CloudTable GetTable(CloudStorageAccount storageAccount, string tableName)
        {
            var sourceTableClient = storageAccount.CreateCloudTableClient();
            return sourceTableClient.GetTableReference(tableName);
        }
    }
}
