using System;

namespace DdhpCore.EventSource.Events
{
    public class FixtureAddedEvent
    {
        public int RoundNumber { get; }
        public Guid HomeClub { get; set; }
        public Guid AwayClub { get; set; }

        public FixtureAddedEvent(int roundNumber, Guid homeClub, Guid awayClub)
        {
            RoundNumber = roundNumber;
            HomeClub = homeClub;
            AwayClub = awayClub;
        }
    }
}