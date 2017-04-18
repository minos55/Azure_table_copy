using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Azure_table_copy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.RollingFile("log-{Date}.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
                    .CreateLogger();
            InitializeComponent();
        
        }

        private void Start_Click(object sender, EventArgs e)
        {
            Start.Enabled = sourceConnectionTextBox.Enabled = sourceTableNameTextBox.Enabled = 
                targetConnectionTextBox.Enabled = targetTableNameTextBox.Enabled = 
                batchSizeNumericUpDown.Enabled = false;

            var progress = new Progress<int>(percent => progressBar1.Value = percent);
            var test = WriteCityWeatherToTableAsync(sourceConnectionTextBox.Text, sourceTableNameTextBox.Text,
                                                    targetConnectionTextBox.Text, targetTableNameTextBox.Text,
                                                    (int)batchSizeNumericUpDown.Value, progress);

            Task.WhenAny(test);
        }

        public async Task WriteCityWeatherToTableAsync(string _sourceConnectionString, string _sourceTableName, string _targetConnectionString, string _targetTableName, int _batchSize, IProgress<int> _progress)
        {
            try
            {
                CloudStorageAccount sourceStorageAccount;
                CloudStorageAccount targetStorageAccount;
                if (CloudStorageAccount.TryParse(_sourceConnectionString, out sourceStorageAccount) && CloudStorageAccount.TryParse(_sourceConnectionString, out targetStorageAccount))
                {
                    CloudTableClient sourceTableClient = sourceStorageAccount.CreateCloudTableClient();
                    CloudTableClient targetTableClient = targetStorageAccount.CreateCloudTableClient();
                    
                    Log.Information("Connected to {Connection}", sourceStorageAccount);
                    Log.Information("Connected to {Connection}", targetStorageAccount);

                    CloudTable sourceTable = sourceTableClient.GetTableReference(_sourceTableName);
                    await sourceTable.CreateIfNotExistsAsync();
                    CloudTable targetTable = sourceTableClient.GetTableReference(_targetTableName);
                    await targetTable.CreateIfNotExistsAsync();


                    var tableQuery = new TableQuery();
                    int totalCount = sourceTable.ExecuteQuery(tableQuery).Count();
                    var progressCount = Enumerable.Repeat(0, totalCount).ToArray();
                    tableQuery.Take(_batchSize);


                    TableContinuationToken continuationToken = null;
                    int tempCount = 0;
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
                                tempCount++;
                                if (_progress != null)
                                {
                                    _progress.Report((tempCount * 100 / totalCount));
                                }
                                
                                Log.Information($"Entity with the PartitionKey = ({item.PartitionKey}) and RowKey = ({item.RowKey}) copied from {_sourceTableName} into table {_targetTableName}.");
                            }
                            await targetTable.ExecuteBatchAsync(batchOperation);
                        }


                        // Loop until a null continuation token is received, indicating the end of the table.
                    } while (continuationToken != null);
                }
                
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
            }
            Start.Enabled = sourceConnectionTextBox.Enabled = sourceTableNameTextBox.Enabled =
                targetConnectionTextBox.Enabled = targetTableNameTextBox.Enabled =
                batchSizeNumericUpDown.Enabled = true;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
