using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Models
{
    public class Fixture : TableEntity
    {
        private int _roundId;
        private Guid _home;
        private Guid _away;

        public int RoundId
        {
            get { return _roundId; }
            set
            {
                PartitionKey = value.ToString();
                _roundId = value;
            }
        }

        public Guid Home
        {
            get { return _home; }
            set
            {
                SetRowKey();
                _home = value;
            }
        }

        public Guid Away
        {
            get { return _away; }
            set
            {
                SetRowKey();
                _away = value;
            }
        }

        private void SetRowKey()
        {
            RowKey = $"{Home}-{Away}";
        }
    }
}