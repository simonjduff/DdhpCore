namespace DdhpCore.Services.EventWriter
{
    public interface IEvent<T>
    {
        string EntityId { get; set; }
        string UniqueEventId { get; set; }
        T Entity { get; set; }
    }
}
