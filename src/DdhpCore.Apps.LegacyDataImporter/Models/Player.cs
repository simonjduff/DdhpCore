using System;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

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

        [Obsolete("For internal use only")]
        public string CurrentAflClubString
        {
            get { return JsonConvert.SerializeObject(_currentAflClub); }
            set { _currentAflClub = JsonConvert.DeserializeObject<AflClub>(value); }
        }

        public bool Active { get; set; }
        public string FootywireName { get; set; }

        public class AflClub
        {
            public string Name { get; set; }
            public string ShortName { get; set; }
        }

        [IgnoreProperty]
        public int LegacyId { get; set; }
    }
}