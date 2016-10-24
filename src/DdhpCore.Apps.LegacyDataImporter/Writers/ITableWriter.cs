using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Writers
{
    public interface ITableWriter<in T> where T : ITableEntity
    {
        ITableWriter<T> WriteData(IEnumerable<T> data);
        ITableWriter<T> ClearTable();
    }

    public class TableWriter<T> : ITableWriter<T> where T : ITableEntity, new()
    {
        protected readonly CloudTable Table;

        public TableWriter(CloudTable table)
        {
            Table = table;
        }

        public ITableWriter<T> WriteData(IEnumerable<T> data)
        {
            var partitions = data.ToLookup(q => q.PartitionKey, q => q);

            foreach (var partition in partitions)
            {
                var insert = new TableBatchOperation();

                partition.ToList().ForEach(datum => insert.Add(TableOperation.Insert(datum)));

                Table.ExecuteBatchAsync(insert).GetAwaiter().GetResult();
            }
            return this;
        }

        public ITableWriter<T> ClearTable()
        {
            var data = GetAllData();

            if (!data.Any())
            {
                return this;
            }
            var partitions = data.ToLookup(q => q.PartitionKey, q => q);

            foreach (var partition in partitions)
            {
                var delete = new TableBatchOperation();

                partition.ToList().ForEach(datum => delete.Add(TableOperation.Delete(datum)));

                Table.ExecuteBatchAsync(delete).GetAwaiter().GetResult();
            }
            return this;
        }

        public IEnumerable<T> GetAllData()
        {
            var operation = new TableQuery<T>();
            TableContinuationToken continuer = null;

            List<T> clubs = new List<T>();

            do
            {
                var tableQuerySegment = Table.ExecuteQuerySegmentedAsync(operation, continuer).Result;
                clubs.AddRange(tableQuerySegment.Results);
                continuer = tableQuerySegment.ContinuationToken;
            } while (continuer != null);

            return clubs;
        }
    }
}