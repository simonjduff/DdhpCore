using System;
using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Models.Api
{
    public class Club
    {
        public Guid Id { get; set; }
        public string CoachName { get; set; }
        public string ClubName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ClubAtRound ClubAtRound { get; set; }
    }
}