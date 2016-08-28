using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyDataImporter.LegacyModels
{
    [Table("Teams", Schema = "ddhp")]
    public class Team
    {
        public int Id { get; set; } 
        public string CoachName { get; set; }
        public  string TeamName { get; set; }
        public string Email { get; set; }
    }
}