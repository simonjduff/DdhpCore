using System.Collections.Generic;
using LegacyDataImporter.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Writers
{
    public class ClubsTableWriter : TableWriter<Club>
    {
        public ClubsTableWriter(CloudTable table) : base(table)
        {
        }

        public override IEnumerable<Club> GetAllData()
        {
            var operation = new TableQuery<Club>();
            TableContinuationToken continuer = null;

            List<Club> clubs = new List<Club>();

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