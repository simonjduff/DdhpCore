using System.Collections.Generic;
using DdhpCore.FrontEnd.Models.Values;

namespace DdhpCore.FrontEnd.Models.Api
{
    public class ClubAtRound
    {
        public RoundValue Round { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
        public PlayedTeam Team { get; set; }
    }
}