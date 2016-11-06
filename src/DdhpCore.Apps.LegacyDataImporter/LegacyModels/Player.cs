using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyDataImporter.LegacyModels
{
    public class Player
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string MiddleNames { get; set; }
        public string FirstName { get; set; }
        public int CurrentAflTeamId { get; set; }
        [ForeignKey("CurrentAFLTeamId")]
        public virtual AflTeam CurrentAflTeam { get; set; }
        public bool Active { get; set; }
        public string FootywireName { get; set; }

    }
}