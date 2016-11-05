using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyDataImporter.LegacyModels
{
    [Table("RoundPlayers", Schema = "ddhp")]
    public class RoundPlayer
    {
        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }
        public string PickedPosition { get; set; }
        public string PlayedPosition { get; set; }
        [Column("Round")]
        public int RoundId { get; set; }
        public virtual Round Round { get; set; }
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
    }
}