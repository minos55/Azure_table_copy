﻿using System.Threading.Tasks;

namespace Nomnio.TableCopyLibrary.Interfaces
{
    public interface ITableCopy
    {
        Task<bool> CopyAsync(string _sourceConnectionString, string _sourceTableName, string _targetConnectionString, string _targetTableName, int _batchSize);
    }
}
