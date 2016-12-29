namespace DdhpCore.EventSource.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Events;
    using Models;
    using Xunit;
    public class ClubTests : EventSourceTestBase
    {
        private static readonly Guid EntityId = Guid.NewGuid();
        private static readonly string ClubName = "Cheats";
        private static readonly string CoachName = "Simon Duff";
        private static readonly string Email = "anemail@nowhere.nowhere";

        private readonly List<TestContract> _contracts = new List<TestContract>();

        public class TestContract
        {
            public Guid PlayerId { get; set; }
            public int FromRound { get; set; }
            public int ToRound { get; set; }
            public int DraftPick { get; set; }
        }

        [Fact]
        public async Task ClubCreatedEventCreatesClub() 
        {
            // Given an entity factory
            GivenAnEntityFactory();

            // And a ClubCreatedEvent
            GivenAClubCreatedEvent();

            // When I ask for a club
            Club club = await EventEntityFactory.ReplayEntity<Club>(EntityId.ToString());

            // Then the club is created correctly
            Assert.NotNull(club);
            Assert.Equal(Email, club.Email);
            Assert.Equal(ClubName, club.ClubName);
            Assert.Equal(CoachName, club.CoachName);
            Assert.Equal(EntityId, club.Id);
        }

        [Fact]
        public async Task ContractImportedEventCreatesContract()
        {
            // Given an entity factory
            GivenAnEntityFactory();

            // And a ClubCreatedEvent
            GivenAClubCreatedEvent();

            // And a ContractImportedEvent
            Guid id = Guid.NewGuid();
            GivenAContractImportedEvent(id, 200801, 200824, 1);

            // When I ask for a club
            Club club = await EventEntityFactory.ReplayEntity<Club>(EntityId.ToString());

            // Then the club has the contract
            Assert.Equal(1, club.Contracts.Count());
            Assert.Equal(id, club.Contracts.First().PlayerId);
            Assert.Equal(200801, club.Contracts.First().FromRound);
            Assert.Equal(200824, club.Contracts.First().ToRound);
            Assert.Equal(1, club.Contracts.First().DraftPick);
            Assert.Equal(1, club.Version);
        }

        private void GivenAContractImportedEvent(Guid playerId, int fromRound, int toRound, int draftPick)
        {
            var testContract = new TestContract
            {
                PlayerId = playerId,
                FromRound = fromRound,
                ToRound = toRound,
                DraftPick = draftPick
            };
            _contracts.Add(testContract);
            var tEvent = new ContractImportedEvent(playerId, fromRound, toRound, draftPick);
            Event importedEvent = new Event(EntityId.ToString(), EntityVersion++, "ContractImported", tEvent);
            Events.Add(importedEvent);
        }

        private void GivenAClubCreatedEvent()
        {
            ClubCreatedEvent cEvent = new ClubCreatedEvent(EntityId, ClubName, CoachName, Email);
            Event createEvent = new Event(EntityId.ToString(), EntityVersion++, "ClubCreated", cEvent);
            Events.Add(createEvent);

        }
    }
}
