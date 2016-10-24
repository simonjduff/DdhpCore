using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Models
{
    public class Round : TableEntity
    {
        private int _year;
        private int _roundNumber;

        public int Id { get; set; }

        public int Year
        {
            get { return _year; }
            set
            {
                PartitionKey = value.ToString();
                _year = value;
            }
        }

        public int RoundNumber
        {
            get { return _roundNumber; }
            set
            {
                RowKey = value.ToString();
                _roundNumber = value;
            }
        }

        public bool RoundComplete { get; set; }
        public bool NormalRound { get; set; }
    }
}