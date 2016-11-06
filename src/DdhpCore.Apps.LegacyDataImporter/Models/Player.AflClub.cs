using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Models
{
    public class AflClub : TableEntity
    {
        private Guid _id;

        public AflClub()
        {
            PartitionKey = "AFL_CLUB";
        }

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
        public string ShortName { get; set; }
    }
}