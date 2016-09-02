using Microsoft.Extensions.Logging;

namespace DdhpCore.Services.EventWriter
{
    public class EventWriter<T> where T : IEvent<T>
    {
        private readonly ILogger<EventWriter<T>> _logger;
        private IStorageTable _table;

        public EventWriter(ILoggerFactory loggerFactory, IStorageTable table)
        {
            _table = table;
            _logger = loggerFactory.CreateLogger<EventWriter<T>>();
        }
    }
}
