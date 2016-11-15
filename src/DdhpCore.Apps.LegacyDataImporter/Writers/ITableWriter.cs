using System.Collections.Generic;
using System.Linq;
using LegacyDataImporter.Extensions;
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
            const int maximumBatchSize = 100;

            foreach (var partition in partitions)
            {
                foreach (var batch in partition.Partition(maximumBatchSize))
                {
                    var insert = new TableBatchOperation();

                    batch.ToList().ForEach(datum => insert.Add(TableOperation.Insert(datum)));

                    Table.ExecuteBatchAsync(insert).GetAwaiter().GetResult();
                }
            }

            return this;
        }

        public ITableWriter<T> ClearTable()
        {
            var data = Table.GetAllData<T>();
            const int batchSize = 100;

            if (!data.Any())
            {
                return this;
            }
            var partitions = data.ToLookup(q => q.PartitionKey, q => q);

            foreach (var partition in partitions)
            {
                foreach (var batch in partition.Partition(batchSize))
                {
                    var delete = new TableBatchOperation();

                    batch.ToList().ForEach(datum => delete.Add(TableOperation.Delete(datum)));

                    Table.ExecuteBatchAsync(delete).GetAwaiter().GetResult();
                }
            }
            return this;
        }
    }
}