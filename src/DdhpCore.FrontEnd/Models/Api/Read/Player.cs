using System;

namespace DdhpCore.FrontEnd.Models.Api.Read
{
    public class Player
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CurrentAflClubId { get; set; }

        public bool Active { get; set; }
        public string FootywireName { get; set; }
    }
}