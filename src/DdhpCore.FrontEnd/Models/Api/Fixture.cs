using System;

namespace DdhpCore.FrontEnd.Models.Api
{
    public class Fixture{
        public int RoundId { get; set; }

        public Guid Home { get; set; }

        public Guid Away { get; set; }
    }
}