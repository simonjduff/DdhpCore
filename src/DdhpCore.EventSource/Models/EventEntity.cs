using System;
using System.Collections.Generic;
using System.Linq;
using DdhpCore.EventSource.Events;

namespace DdhpCore.EventSource.Models
{
    public abstract class EventEntity
    {
        protected EventEntity()
        {
            Version = -1;
        }
        protected abstract void ReplayEvent(Event e);
        public int Version { get; private set; }

        public static T LoadFromEvents<T>(IEnumerable<Event> events) where T : EventEntity, new()
        {
            var entity = new T();

            foreach (var e in events.OrderBy(q => q.RowKey))
            {
                entity.ReplayEvent(e);

                var version = int.Parse(e.RowKey);
                if (version != entity.Version + 1)
                {
                    throw new Exception($"Events out of order. At version {entity.Version} but received event {version}");
                }
                entity.Version = version;
            }

            return entity;
        }
    }
}