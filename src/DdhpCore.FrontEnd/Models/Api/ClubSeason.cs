using System;
using System.Collections.Generic;
using DdhpCore.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Models.Api
{
    [TableName("clubsRead")]
    public class ClubSeason : TableEntity
    {
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

        private List<Contract> _contracts = new List<Contract>();

        public string Contracts
        {
            get { return JsonConvert.SerializeObject(_contracts); }
            set { _contracts = (List<Contract>)JsonConvert.DeserializeObject<List<Contract>>(value); }
        }

        public int Version { get; set; }
    }
}