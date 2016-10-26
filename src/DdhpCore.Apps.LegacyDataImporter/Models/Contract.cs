using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Models
{
    public class Contract : TableEntity
    {
        public int FromRound { get; set; }
        public int ToRound { get; set; }
        public Guid PlayerId { get; set; }
        public int DraftPick { get; set; }
        public Guid ClubId { get; set; }
    }
}