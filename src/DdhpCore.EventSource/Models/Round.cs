using System.Collections.Generic;

namespace DdhpCore.EventSource.Models
{
    public class Round
    {
        public Round()
        {
            Fixtures = new List<Fixture>();
        }
        public int RoundNumber { get; set; }
        public bool LadderRound { get; set;  }
        public bool Completed { get; set;  }
        public IList<Fixture> Fixtures { get; set; }
    }
}