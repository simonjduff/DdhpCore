using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyDataImporter.LegacyModels
{
    [Table("Teams", Schema = "afl")]
    public class AflTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}