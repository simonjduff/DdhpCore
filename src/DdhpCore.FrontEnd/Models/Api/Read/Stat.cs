using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Models.Api.Read
{
    public class Stat
    {
        [JsonProperty("rn")]
        public int RoundNumber { get; set; }
        [JsonProperty("f")]
        public int Forward { get; set; }
        [JsonProperty("m")]
        public int Midfield { get; set; }
        [JsonProperty("r")]
        public int Ruck { get; set; }
        [JsonProperty("t")]
        public int Tackle { get; set; }
    }
}