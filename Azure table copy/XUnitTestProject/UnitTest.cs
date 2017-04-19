using Xunit;
using TableCopyLibrary;
using System.Threading.Tasks;
using System;

namespace XUnitTestProject
{
    public class UnitTest
    {
        const string sourceConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
        const string sourceTableName = "weathertable";
        const string targetConnectionString = "DefaultEndpointsProtocol=https;AccountName=mt1;AccountKey=O9+FoFPCQ4wqqfMJLm5I1zp7sePAgGGfowvDmCnGBt+AKlrdTXGOJ8QuzoQWz7yTsKPiOvBRE/8PfW5kRzzsTg==;EndpointSuffix=core.windows.net";
        const string targetTableName = "weathertable2";
        const int batchSize = 5;

        [Theory]
        [InlineData(sourceConnectionString, sourceTableName, targetConnectionString, targetTableName, batchSize)]
        [InlineData(sourceConnectionString, sourceTableName, targetConnectionString, "WeatherTableTest", batchSize)]
        [InlineData(sourceConnectionString, "WeatherTableUnitTest", targetConnectionString, targetTableName, batchSize)]
        public async Task PassingTest(string _sourceConnectionString, string _sourceTableName, string _targetConnectionString, string _targetTableName, int _batchSize)
        {
            var obj = new TableCopy();
            var test = await obj.CopyAsync(_sourceConnectionString, _sourceTableName, _targetConnectionString, _targetTableName, _batchSize);

            Assert.True(test);
        }

        [Theory]
        [InlineData("test", sourceTableName, targetConnectionString, targetTableName + " .", 5)]
        [InlineData(sourceConnectionString, sourceTableName, targetConnectionString, targetTableName+" .", 5)]
        public async Task FailingTest(string _sourceConnectionString, string _sourceTableName, string _targetConnectionString, string _targetTableName, int _batchSize)
        {
            var obj = new TableCopy();
            var test = await obj.CopyAsync(_sourceConnectionString, _sourceTableName, _targetConnectionString, _targetTableName, _batchSize);

            Assert.False(test);
        }
    }
}
