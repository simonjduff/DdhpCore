using System;
using DdhpCore.FrontEnd.Models.Values;

namespace DdhpCore.FrontEnd.Models.Api
{
    public class Contract
    {
        public Guid PlayerId { get; set; }
        public RoundValue FromRound { get; set; }
        public RoundValue ToRound { get; set; }
        public Player Player { get; set; }
        public DraftPick DraftPick { get; set; }
    }
}