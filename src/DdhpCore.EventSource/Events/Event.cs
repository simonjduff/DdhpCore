using System;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace DdhpCore.EventSource.Events
{
    public class Event : TableEntity
    {
        public Event(Guid entityId,
            int entityVersion,
            string eventType,
            object payload)
        {
            RowKey = entityVersion.ToString("0000000000");
            PartitionKey = entityId.ToString();
            EventType = eventType;
            SetPayload(payload);
        }

        public Event()
        {
        }

        public string EventType { get; set; }
        internal string Payload { get; set; }
        public T GetPayload<T>()
        {
            return (T)JsonConvert.DeserializeObject<T>(Payload);
        }

        public void SetPayload(object payload)
        {
            Payload = JsonConvert.SerializeObject(payload);
        }
    }
}