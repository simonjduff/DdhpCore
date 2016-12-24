using System;
using DdhpCore.FrontEnd.Configuration;
using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Models.Values
{
    [JsonConverter(typeof(JsonStructConverter))]
    public struct RoundValue
    {
        public RoundValue(int round)
        {
            int year = round/100;
            int roundNumber = round - year*100;

            if (year < 2008 || year > 2040 || roundNumber < 0 || roundNumber > 24)
            {
                throw new ArgumentOutOfRangeException($"Round {round} is not in the appropriate range of 200801 and 204024");
            }

            _round = round;
            Year = year;
            RoundNumber = roundNumber;
        }

        private readonly int _round;
        public int RoundNumber { get; }
        public int Year { get; }


        public static implicit operator RoundValue(int round)
        {
            return new RoundValue(round);
        }

        public static implicit operator int(RoundValue round)
        {
            return round._round;
        }

        public override string ToString()
        {
            return _round.ToString();
        }
    }
}