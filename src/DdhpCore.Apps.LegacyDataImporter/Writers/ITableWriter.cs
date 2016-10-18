using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Writers
{
    public interface ITableWriter<in T> where T : ITableEntity
    {
        void WriteData(IEnumerable<T> data);
        void ClearTable();
    }

    public abstract class TableWriter<T> : ITableWriter<T> where T : ITableEntity
    {
        protected readonly CloudTable Table;

        protected TableWriter(CloudTable table)
        {
            Table = table;
        }

        public void WriteData(IEnumerable<T> data)
        {
            var insert = new TableBatchOperation();

            data.ToList().ForEach(datum => insert.Add(TableOperation.Insert(datum)));

            Table.ExecuteBatchAsync(insert).Wait();
        }

        public void ClearTable()
        {
            var data = GetAllData().ToList();

            if (!data.Any())
            {
                return;
            }

            var delete = new TableBatchOperation();

            data.ForEach(datum => delete.Add(TableOperation.Delete(datum)));

            Table.ExecuteBatchAsync(delete).Wait();
        }

        public abstract IEnumerable<T> GetAllData();
    }
}