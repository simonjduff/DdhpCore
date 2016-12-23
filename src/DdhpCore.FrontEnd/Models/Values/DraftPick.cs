using System;
using DdhpCore.FrontEnd.Configuration;
using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Models.Values
{
    [JsonConverter(typeof(JsonStructConverter))]
    public struct DraftPick
    {
        public DraftPick(int pick)
        {
            if (pick < 1 || pick > 24)
            {
                throw new ArgumentOutOfRangeException($"Draft pick {pick} is not in the appropriate range of 1 - 24");
            }

            _pick = pick;
        }

        private readonly int _pick;

        public static explicit operator DraftPick(int pick)
        {
            return new DraftPick(pick);
        }

        public static implicit operator int(DraftPick pick)
        {
            return pick._pick;
        }

        public override string ToString()
        {
            return _pick.ToString();
        }
    }
}