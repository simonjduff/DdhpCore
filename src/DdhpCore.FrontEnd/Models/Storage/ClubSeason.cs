using System;
using System.Collections.Generic;
using System.Linq;
using DdhpCore.Storage;

namespace DdhpCore.FrontEnd.Models.Storage
{
    [TableName("SeasonStatsByClub")]
    public class ClubSeason : ComplexEntity
    {
        public ClubSeason()
        {
            Contracts = Enumerable.Empty<Contract>();
        }

        public Guid Id
        {
            get { return Guid.Parse(PartitionKey); }
            set
            {
                PartitionKey = value.ToString();
            }
        }

        public int Year
        {
            get
            {
                return int.Parse(RowKey);
            }
            set
            {
                RowKey = value.ToString();
            }
        }

        [Serialize]
        public IEnumerable<Contract> Contracts { get; set; }

        public int Version { get; set; }
    }
}