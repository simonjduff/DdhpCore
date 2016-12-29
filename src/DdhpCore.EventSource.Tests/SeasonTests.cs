using System;
using DdhpCore.EventSource.Models;

namespace DdhpCore.EventSource.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Events;
    using Xunit;

    public class SeasonTests : EventSourceTestBase
    {
        private static readonly int EntityId = 2008;

        [Fact]
        public async Task SeasonCreatedEventCreatesSeason()
        {
            // Given an entity factory
            GivenAnEntityFactory();

            // And a SeasonCreatedEvent
            CreateSeason();

            // When asked for a season
            Season season = await EventEntityFactory.ReplayEntity<Season>(EntityId.ToString());

            // Then the season is created correctly
            Assert.Equal(2008, season.Id);
        }

        [Fact]
        public async Task RoundAddedEventAddsRound()
        {
            // Given an entity factory
            GivenAnEntityFactory();

            // And a SeasonCreatedEvent
            CreateSeason();

            // And a RoundCreatedEvent
            CreateRound(1);

            // When asked for a season
            Season season = await EventEntityFactory.ReplayEntity<Season>(EntityId.ToString());

            // Then the season has the round
            Assert.Equal(1, season.Rounds.Count());
            Assert.Equal(1, season.Rounds.First().RoundNumber);
            Assert.Equal(true, season.Rounds.First().LadderRound);
            Assert.Equal(false, season.Rounds.First().Completed);
        }

        [Fact]
        public async Task FixtureAddedEventAddsFixtureToRound()
        {
            // Given an entity factory
            GivenAnEntityFactory();

            // And a SeasonCreatedEvent
            CreateSeason();

            // And a RoundCreatedEvent
            var roundNumber = 1;
            CreateRound(roundNumber);

            // And a FixtureAddedEvent
            Guid homeClub = Guid.NewGuid();
            Guid awayClub = Guid.NewGuid();
            CreateFixture(roundNumber, homeClub, awayClub);

            // When asked for a season
            Season season = await EventEntityFactory.ReplayEntity<Season>(EntityId.ToString());

            // Then the fixture is in the round
            var round = season.Rounds.Single(q => q.RoundNumber == roundNumber);
            Assert.Equal(1, round.Fixtures.Count());
            Assert.Equal(homeClub, round.Fixtures.First().HomeClub);
            Assert.Equal(awayClub, round.Fixtures.First().AwayClub);
        }

        private void CreateFixture(int roundNumber,
            Guid homeClub, 
            Guid awayClub)
        {
            FixtureAddedEvent fEvent = new FixtureAddedEvent(roundNumber, homeClub, awayClub);
            Event fixtureEvent = new Event(EntityId.ToString(), EntityVersion++, "FixtureAdded", fEvent);
            Events.Add(fixtureEvent);
        }

        private void CreateSeason()
        {
            SeasonCreatedEvent cEvent = new SeasonCreatedEvent(EntityId);
            Event createEvent = new Event(EntityId.ToString(), EntityVersion++, "SeasonCreated", cEvent);
            Events.Add(createEvent);
        }

        private void CreateRound(int roundNumber)
        {
            RoundAddedEvent cEvent = new RoundAddedEvent(roundNumber, true, false);
            Event createEvent = new Event(EntityId.ToString(), EntityVersion++, "RoundAdded", cEvent);
            Events.Add(createEvent);
        }
    }
}