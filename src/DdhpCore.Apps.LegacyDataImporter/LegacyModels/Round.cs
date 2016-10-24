using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyDataImporter.LegacyModels
{
    [Table("Rounds", Schema = "ddhp")]
    public class Round
    {
        public int Id { get; set; }
        public int Year { get; set; }
        [Column("Round")]
        public int RoundNumber { get; set; }
        public bool RoundComplete { get; set; }
        public bool NormalRound { get; set; }
    }
}