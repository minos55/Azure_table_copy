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
                CloudStorageAccount sourceStorageAccount;
                CloudStorageAccount targetStorageAccount;
                bool source = CloudStorageAccount.TryParse(_sourceConnectionString, out sourceStorageAccount);
                bool target = CloudStorageAccount.TryParse(_sourceConnectionString, out targetStorageAccount);

                if (!source)
                {
                    throw new Exception("Source connection string is wrong.");
                }

                if (!target)
                {
                    throw new Exception("Target connection string is wrong.");
                }

                var sourceTableClient = sourceStorageAccount.CreateCloudTableClient();

                Log.Information("Connected to {Connection}", sourceStorageAccount);

                var targetTableClient = targetStorageAccount.CreateCloudTableClient();

                Log.Information("Connected to {Connection}", targetStorageAccount);

                var sourceTable = sourceTableClient.GetTableReference(_sourceTableName);
                await sourceTable.CreateIfNotExistsAsync();
                var targetTable = sourceTableClient.GetTableReference(_targetTableName);
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
    }
}
