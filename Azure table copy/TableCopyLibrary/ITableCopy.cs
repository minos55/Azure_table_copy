using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TableCopyLibrary
{
    public interface ITableCopy
    {
        Task<bool> CopyAsync(string _sourceConnectionString, string _sourceTableName, string _targetConnectionString, string _targetTableName, int _batchSize);
    }
}
