using System;
using System.Collections.Generic;
using DdhpCore.FrontEnd.Controllers.Api;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Models.Api
{
    public class PlayedTeam : TableEntity
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

        [JsonIgnore]
        public string TeamJson
        {
            get { return JsonConvert.SerializeObject(Team); }
            set { Team = (IEnumerable<PlayedTeam.TeamPlayer>)JsonConvert.DeserializeObject<IEnumerable<PlayedTeam.TeamPlayer>>(value); }
        }

        public int Score { get; set; }

        [IgnoreProperty]
        public IEnumerable<PlayedTeam.TeamPlayer> Team { get; set; }

        public class TeamPlayer
        {
            public Guid PlayerId { get; set; }
            public char PickedPosition { get; set; }
            public char PlayedPosition { get; set; }
            public Stat Stat { get; set; }
            public int Score { get; set; }
        }
    }
}