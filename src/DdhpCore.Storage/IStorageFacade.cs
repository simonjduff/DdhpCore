using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DdhpCore.Storage
{
    public interface IStorageFacade
    {
        Task<IEnumerable<T>> BatchQuery<T>(TableQuery<T> query) where T : ITableEntity, new();
        Task<T> Retrieve<T>(string partitionKey, string rowKey) where T : class, ITableEntity, new();
        Task<IEnumerable<T>> GetAllByPartition<T>(string partitionValue) where T : class, ITableEntity, new();
        Task<IEnumerable<T>> GetRowsInPartition<T>(string partitionValue, IEnumerable<string> rowKeys) where T : class, ITableEntity, new();
    }
}