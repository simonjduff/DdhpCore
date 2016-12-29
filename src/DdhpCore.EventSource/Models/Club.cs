using System;
using System.Collections.Generic;
using DdhpCore.EventSource.Events;

namespace DdhpCore.EventSource.Models
{
    public class Club : EventEntity
    {
        public Guid Id { get; private set; }
        public string CoachName { get; private set; }
        public string ClubName { get; private set; }
        public string Email { get; private set; }
        private readonly List<Contract> _contracts = new List<Contract>();
        public IEnumerable<Contract> Contracts => _contracts;

        protected override void ReplayEvent(Event e)
        {

            ClubEvent type;
            if (!Enum.TryParse(e.EventType, true, out type))
            {
                throw new Exception($"Could not identify ClubEvent type {e.EventType}");
            }

            switch (type)
            {
                case ClubEvent.ClubCreated:
                    var castEvent = e.GetPayload<ClubCreatedEvent>();
                    Id = castEvent.Id;
                    Email = castEvent.Email;
                    CoachName = castEvent.CoachName;
                    ClubName = castEvent.ClubName;
                    return;
                case ClubEvent.ContractImported:
                    var contractImportedEvent = e.GetPayload<ContractImportedEvent>();
                    _contracts.Add(new Contract(contractImportedEvent.PlayerId, 
                        contractImportedEvent.FromRound, 
                        contractImportedEvent.ToRound, 
                        contractImportedEvent.DraftPick));
                    return;
            }
        }

        private enum ClubEvent
        {
            ClubCreated,
            ContractImported
        }
    }
}