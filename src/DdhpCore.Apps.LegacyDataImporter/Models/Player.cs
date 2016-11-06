using System;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace LegacyDataImporter.Models
{
    public partial class Player : TableEntity
    {
        private Guid _id;
        private Guid _currentAflClub;

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

        public Guid CurrentAflClubId
        {
            get { return _currentAflClub; }
            set
            {
                PartitionKey = value.ToString();
                _currentAflClub = value;
            }
        }

        public bool Active { get; set; }
        public string FootywireName { get; set; }

        [IgnoreProperty]
        public int LegacyId { get; set; }
    }
}