using System;
using System.Collections.Generic;
using DdhpCore.FrontEnd.Models.Api.Read;
using Newtonsoft.Json;
using System.Linq;
using AutoMapper;
using DdhpCore.FrontEnd.Configuration;
using DdhpCore.FrontEnd.Models.Storage;
using Xunit;

namespace MapperTests
{
    public class Tests
    {
        [Fact]
        public void MapperTranslatesToApiObject()
        {
            // Given the storage json
            string input = @"[{""id"":""061dc5df-c9f5-4e78-be65-9f4c1067bc46"",
                            ""details"":{""clubName"":""Western Suburbs Walkabout"",""coachName"":""Geoff and Maria Pocock""},
                            ""partitionKey"":""Western Suburbs Walkabout"",""rowKey"":""061dc5df-c9f5-4e78-be65-9f4c1067bc46"",
                            ""timestamp"":""2017-05-15T11:13:29.203+00:00"",
                            ""eTag"":""W/\""datetime'2017-05-15T11%3A13%3A29.203Z'\""""}]";
            IEnumerable<Club> clubs = JsonConvert.DeserializeObject<IEnumerable<Club>>(input);

            // When the mapper is run
            var config = new MapperConfiguration(ClassMaps.BuildMaps);
            var mapper = config.CreateMapper();
            var result = mapper.Map<IEnumerable<ClubApi>>(clubs).ToArray();

            // Then the result is correct
            Assert.Equal(1, result.Length);
            Assert.Equal("Western Suburbs Walkabout", result[0].ClubName);
            Assert.Equal(Guid.Parse("061dc5df-c9f5-4e78-be65-9f4c1067bc46"), result[0].Id);
            Assert.Equal("Geoff and Maria Pocock", result[0].CoachName);
        }
    }
}
