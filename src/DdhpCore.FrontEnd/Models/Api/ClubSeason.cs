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

        private List<Storage.Models.Contract> _contracts = new List<Storage.Models.Contract>();

        public string Contracts
        {
            get { return JsonConvert.SerializeObject(_contracts); }
            set { _contracts = (List<Storage.Models.Contract>)JsonConvert.DeserializeObject<List<Storage.Models.Contract>>(value); }
        }

        public int Version { get; set; }
    }
}