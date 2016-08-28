using LegacyDataImporter.LegacyModels;
using Microsoft.EntityFrameworkCore;

namespace LegacyDataImporter
{
    public class DdhpContext : DbContext
    {
        private readonly string _connectionString;

        public DdhpContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}