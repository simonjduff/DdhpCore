using System;
using DdhpCore.FrontEnd.Models.Values;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace DdhpCore.FrontEnd.Configuration
{
    public class JsonStructConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("CanRead is false so this should never execute");
        }

        public override bool CanRead => false;

        private static readonly Type[] Types =
        {
            typeof(RoundValue),
            typeof(DraftPick)
        };

        public override bool CanConvert(Type objectType)
        {
            return Types.Any(t => t == objectType);
        }
    }
}