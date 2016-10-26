using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LegacyDataImporter.LegacyModels
{
    public class DdhpContext : DbContext
    {
        private readonly string _connectionString;

        public DdhpContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<AflTeam> AflTeams { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}