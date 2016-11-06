using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyDataImporter.LegacyModels
{
    [Table("Stats")]
    public class Stat
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Round { get; set; }
        public int Goals { get; set; }
        public int Behinds { get; set; }
        public int Disposals { get; set; }
        public int Marks { get; set; }
        public int Hitouts { get; set; }
        public int Tackles { get; set; }
        public int? Kicks { get; set; }
        public int? Handballs { get; set; }
        public int? GoalAssists { get; set; }
        // ReSharper disable once InconsistentNaming
        public int? Inside50s { get; set; }
        public int? FreesFor { get; set; }
        public int? FreesAgainst { get; set; }
        // ReSharper disable once InconsistentNaming
        public int AFLTeamId { get; set; }
        public virtual AflTeam AflTeam { get; set; }
    }
}