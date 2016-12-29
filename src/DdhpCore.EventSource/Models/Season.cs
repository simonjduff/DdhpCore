using System;
using System.Collections.Generic;
using DdhpCore.EventSource.Events;

namespace DdhpCore.EventSource.Models
{
    public class Season : EventEntity
    {
        protected override void ReplayEvent(Event e)
        {
            SeasonEvent type;
            if (!Enum.TryParse(e.EventType, true, out type))
            {
                throw new Exception($"Could not identify SeasonEvent type {e.EventType}");
            }

            switch (type)
            {
                case SeasonEvent.SeasonCreated:
                    var seasonCreated = e.GetPayload<SeasonCreatedEvent>();
                    Id = seasonCreated.Year;
                    return;
                case SeasonEvent.RoundAdded:
                    var roundAdded = e.GetPayload<RoundAddedEvent>();
                    _rounds.Add(new Round
                    {
                        RoundNumber = roundAdded.RoundNumber,
                        LadderRound = roundAdded.LadderRound,
                        Completed = roundAdded.Completed
                    });
                    return;
            }
        }

        private readonly List<Round> _rounds = new List<Round>();
        public int Id { get; set; }
        public IEnumerable<Round> Rounds => _rounds;

        private enum SeasonEvent
        {
            SeasonCreated,
            RoundAdded
        }
    }

    public class Round
    {
        public int RoundNumber { get; set; }
        public bool LadderRound { get; set; }
        public bool Completed { get; set; }
    }
}