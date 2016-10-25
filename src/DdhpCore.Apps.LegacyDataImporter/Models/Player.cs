using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Models
{
    public class Player : TableEntity
    {
        private Guid _id;
        private AflClub _currentAflClub;

        public Guid Id
        {
            get { return _id; }
            set
            {
                RowKey = value.ToString();
                _id = value;
            }
        }

        public string Name { get; set; }

        public AflClub CurrentAflClub
        {
            get { return _currentAflClub; }
            set
            {
                PartitionKey = value.ShortName;
                _currentAflClub = value;
            }
        }

        public bool Active { get; set; }
        public string FootywireName { get; set; }

        public class AflClub
        {
            public string Name { get; set; }
            public string ShortName { get; set; }
        }
    }
}