using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DdhpCore.EventSource.Events;
using DdhpCore.EventSource.Models;
using DdhpCore.Storage;
using NSubstitute;
using Xunit;

namespace DdhpCore.EventSource.Tests
{
    public class Tests
    {
        private EventEntityFactory _eventEntityFactory;
        private IStorageFacade _storageFacade;

        public static readonly string ClubName = "Cheats";
        private static readonly string CoachName = "Simon Duff";
        private static readonly string Email = "anemail@nowhere.nowhere";
        private static readonly Guid Id = Guid.NewGuid();
        private int _entityVersion = 0;

        private readonly List<TestContract> _contracts = new List<TestContract>();
        private readonly List<Event> _events = new List<Event>();

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
            Club club = await _eventEntityFactory.ReplayEntity<Club>(Id.ToString());

            // Then the club is created correctly
            Assert.NotNull(club);
            Assert.Equal(Email, club.Email);
            Assert.Equal(ClubName, club.ClubName);
            Assert.Equal(CoachName, club.CoachName);
            Assert.Equal(Id, club.Id);
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
            Club club = await _eventEntityFactory.ReplayEntity<Club>(Id.ToString());

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
            Event importedEvent = new Event(Id, _entityVersion++, "ContractImported", tEvent);
            _events.Add(importedEvent);
        }

        private void GivenAClubCreatedEvent()
        {
            ClubCreatedEvent cEvent = new ClubCreatedEvent(Id, ClubName, CoachName, Email);
            Event createEvent = new Event(Id, _entityVersion++, "ClubCreated", cEvent);
            _events.Add(createEvent);

        }

        private void GivenAnEntityFactory()
        {
            _storageFacade = Substitute.For<IStorageFacade>();
            _storageFacade.GetAllByPartition<Event>(Id.ToString(), string.Empty).Returns(_events);
            _eventEntityFactory = new EventEntityFactory(_storageFacade);
        }
    }
}
