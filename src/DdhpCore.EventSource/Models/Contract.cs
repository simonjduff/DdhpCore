using System;

namespace DdhpCore.EventSource.Models
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
        public Guid PlayerId { get; }
        public int FromRound { get; }
        public int ToRound { get; }
        public int DraftPick { get; }
    }
}