using System;
using System.Collections.Generic;
using System.Linq;
using DdhpCore.Storage;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Models.Api.Read
{
    [TableName("clubsRead")]
    public class ClubSeason : ComplexEntity
    {
        public ClubSeason()
        {
            Contracts = Enumerable.Empty<Contract>();
        }

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RowKey = value.ToString();
            }
        }

        public string CoachName { get; set; }
        public string ClubName { get; set; }
        public string Email { get; set; }
        public int Year
        {
            get
            {
                return int.Parse(PartitionKey);
            }
            set
            {
                PartitionKey = value.ToString();
            }
        }

        [Serialize]
        public IEnumerable<Contract> Contracts { get; set; }

        public int Version { get; set; }
    }
}