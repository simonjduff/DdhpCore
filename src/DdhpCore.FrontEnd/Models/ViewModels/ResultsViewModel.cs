using System;
using System.Collections.Generic;
using DdhpCore.FrontEnd.Models.Api;

namespace DdhpCore.FrontEnd.Models.ViewModels
{
    public struct ResultsViewModel
    {
        private int _round;
        public int Round
        {
            set { _round = value; }
        }

        public int RoundNumber => int.Parse(_round.ToString().Substring(4, 2));
        public int RoundYear => int.Parse(_round.ToString().Substring(0, 4));
        public IEnumerable<Fixture> Fixtures { get; set; }
        public IDictionary<Guid, PlayedTeam> PlayedTeams { get; set; }
    }
}