using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace LegacyDataImporter.Models
{
    public class PickedTeam : TableEntity
    {
        private Guid _clubId;
        private int _round;
        public Guid Id { get; set; }

        public Guid ClubId
        {
            get { return _clubId; }
            set
            {
                RowKey = value.ToString();
                _clubId = value;
            }
        }

        public int Round
        {
            get { return _round; }
            set
            {
                PartitionKey = value.ToString();
                _round = value;
            }
        }

        public string TeamJson
        {
            get { return JsonConvert.SerializeObject(Team); }
            set { Team = (IEnumerable<TeamPlayer>) JsonConvert.DeserializeObject<IEnumerable<TeamPlayer>>(value); }
        }

        [IgnoreProperty]
        public IEnumerable<TeamPlayer> Team { get; set; }

        public class TeamPlayer
        {
            public Guid PlayerId { get; set; }
            public char PickedPosition { get; set; }
        }
    }
}