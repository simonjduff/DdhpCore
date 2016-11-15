using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Extensions
{
    public static class CloudTableExtensions
    {
        public static IEnumerable<T> GetAllData<T>(this CloudTable cloudTable) where T : ITableEntity, new()
        {
            var operation = new TableQuery<T>();
            TableContinuationToken continuer = null;

            do
            {
                var tableQuerySegment = cloudTable.ExecuteQuerySegmentedAsync(operation, continuer).Result;

                foreach (var result in tableQuerySegment.Results)
                {
                    yield return result;
                }

                continuer = tableQuerySegment.ContinuationToken;
            } while (continuer != null);
        }
    }
}