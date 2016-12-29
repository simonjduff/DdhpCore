using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace DdhpCore.Storage
{
    public class StorageFacade : IStorageFacade
    {
        private readonly CloudTableClient _tableClient;
        private readonly ILogger<StorageFacade> _logger;
        private const string PartitionKey = "PartitionKey";
        private const string RowKey = "RowKey";

        public StorageFacade(CloudTableClient tableClient,
            ILoggerFactory loggerFactory)
        {
            _tableClient = tableClient;
            _logger = loggerFactory.CreateLogger<StorageFacade>();
        }

        public async Task<IEnumerable<T>> BatchQuery<T>(TableQuery<T> query) where T : ITableEntity, new()
        {
            var tableName = TableName(typeof(T));

            return await BatchQuery(query, tableName);
        }

        public async Task<IEnumerable<T>> BatchQuery<T>(TableQuery<T> query, string tableName) where T : ITableEntity, new()
        {
            TableContinuationToken continuation = null;

            var results = new List<T>();

            var table = _tableClient.GetTableReference(tableName);

            do
            {
                var result = await table.ExecuteQuerySegmentedAsync(query, continuation);
                results.AddRange(result.Results as IEnumerable<T>);
                continuation = result.ContinuationToken;
            } while (continuation != null);

            return results;
        }

        public async Task<T> Retrieve<T>(string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            var table = _tableClient.GetTableReference(TableName(typeof(T)));

            var query = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(query);
            return result.Result as T;
        }

        public async Task<IEnumerable<T>> GetAllByPartition<T>(string partitionValue) where T : class, ITableEntity, new()
        {
            return await GetAllByPartition<T>(partitionValue, TableName(typeof(T)));
        }

        public async Task<IEnumerable<T>> GetAllByPartition<T>(string partitionValue, string tableName) where T : class, ITableEntity, new()
        {
            var query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition(PartitionKey,
                    QueryComparisons.Equal, partitionValue));

            return await BatchQuery(query, tableName);
        }

        public async Task<IEnumerable<T>> GetRowsInPartition<T>(string partitionValue, IEnumerable<string> rowKeys)
            where T : class, ITableEntity, new()
        {
            List<List<string>> rowKeySets = new List<List<string>>();
            var remaining = rowKeys;
            do
            {
                rowKeySets.Add(new List<string>(remaining.Take(15)));
                remaining = remaining.Skip(15);
            } while (remaining.Any());

            List<T> results = new List<T>(rowKeys.Count());

            foreach (var set in rowKeySets)
            {
                var partitionFilter = TableQuery.GenerateFilterCondition(PartitionKey, QueryComparisons.Equal, partitionValue);

                var filters = TableQuery.GenerateFilterCondition(RowKey, QueryComparisons.Equal, set.First());
                foreach (var key in set.Skip(1))
                {
                    filters = TableQuery.CombineFilters(filters, TableOperators.Or,
                        TableQuery.GenerateFilterCondition(RowKey, QueryComparisons.Equal, key));
                }

                string query = $"{partitionFilter} {TableOperators.And} ({filters})";

                results.AddRange(await BatchQuery<T>(new TableQuery<T>().Where(query)));
            }

            return results;
        }

        private string TableName(Type type)
        {
            var info = type.GetTypeInfo();
            var attribute = info.GetCustomAttribute<TableNameAttribute>();

            if (string.IsNullOrWhiteSpace(attribute?.TableName))
            {
                _logger.LogError($"Could not find the TableName attribute on class {type.FullName}");
                throw new Exception($"Could not find the TableName attribute on class {type.FullName}");
            }

            return attribute.TableName;
        }
    }
}
