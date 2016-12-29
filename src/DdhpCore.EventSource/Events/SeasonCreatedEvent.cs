namespace DdhpCore.EventSource.Events
{
    public class SeasonCreatedEvent
    {
        public int Year { get; }

        public SeasonCreatedEvent(int year)
        {
            Year = year;
        }
    }
}