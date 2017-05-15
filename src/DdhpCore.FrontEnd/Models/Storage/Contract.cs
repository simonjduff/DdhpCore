using System;
using System.Collections.Generic;

namespace DdhpCore.FrontEnd.Models.Storage
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
        }
        public Guid PlayerId { get; set; }
        public int FromRound { get; set; }
        public int ToRound { get; set; }
        public int DraftPick { get; set; }
        public string PlayerName { get; set; }
        public IDictionary<int, IDictionary<char, int>> Stats { get; set; }
    }
}