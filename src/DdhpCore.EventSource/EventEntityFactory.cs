using System.Threading.Tasks;
using DdhpCore.EventSource.Events;
using DdhpCore.EventSource.Models;
using DdhpCore.Storage;

namespace DdhpCore.EventSource
{
    public class EventEntityFactory
    {
        private readonly IStorageFacade _storage;

        public EventEntityFactory(IStorageFacade storage)
        {
            _storage = storage;
        }
        public async Task<T> ReplayEntity<T>(string id) where T: EventEntity, new()
        {
            var events = await _storage.GetAllByPartition<Event>(id, string.Empty);

            var entity = EventEntity.LoadFromEvents<T>(events);

            return entity;
        }
    }
}