using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyDataImporter.LegacyModels
{
    [Table("Contracts", Schema = "ddhp")]
    public class Contract
    {
        public int Id { get; set; }
        public int FromRound { get; set; }
        public int ToRound { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public int DraftPick { get; set; }
    }
}