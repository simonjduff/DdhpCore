using System;
using System.Collections.Generic;
using System.Linq;

namespace DdhpCore.FrontEnd.Models.Api.Read
{
    public class Contract
    {
        public Contract(Guid playerId,
        int fromRound,
        int toRound,
        int draftPick)
        {
            PlayerId = playerId;
            FromRound = fromRound;
            ToRound = toRound;
            DraftPick = draftPick;
            Stats = Enumerable.Empty<Stat>();
        }
        public Guid PlayerId { get; set; }
        public int FromRound { get; set; }
        public int ToRound { get; set; }
        public int DraftPick { get; set; }
        public Player Player { get; set; }
        public IEnumerable<Stat> Stats { get; set; }
    }
}