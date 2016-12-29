namespace DdhpCore.EventSource.Events
{
    public class RoundAddedEvent
    {
        public int RoundNumber { get; }
        public bool LadderRound { get; set; }
        public bool Completed { get; set; }

        public RoundAddedEvent(int roundNumber,
            bool ladderRound,
            bool completed)
        {
            RoundNumber = roundNumber;
            LadderRound = ladderRound;
            Completed = completed;
        }
    }
}