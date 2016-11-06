using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyDataImporter.LegacyModels
{
    [Table("fixtures", Schema="ddhp")]
    public class Fixture
    {
        public int Id { get; set; }
        public int Round { get; set; }
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }
    }
}