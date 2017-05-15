using System;
using System.Collections.Generic;
using DdhpCore.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Models.Storage
{
    [TableName("ClubsRead")]
    public class Club : TableEntity
    {
        public Guid Id
        {
            get { return Guid.Parse(RowKey); }
            set { RowKey = value.ToString(); }
        }
        
        public ClubRow Details { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            this.Details = JsonConvert.DeserializeObject<ClubRow>(properties["Club"].StringValue);
        }

        public class ClubRow
        {
            public string ClubName { get; set; }
            public string CoachName { get; set; }
        }
    }
}