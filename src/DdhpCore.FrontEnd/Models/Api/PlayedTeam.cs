using System;
using System.Collections.Generic;

namespace DdhpCore.FrontEnd.Models.Api
{
    public class PlayedTeam
    {
        public Guid Id { get; set; }

        public Guid ClubId { get; set; }

        public int Round { get; set; }

        public int Score { get; set; }

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