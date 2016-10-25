using System;
using AutoMapper;
using LegacyDataImporter.Importers;
using LegacyDataImporter.LegacyModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using LegacyPlayer = LegacyDataImporter.LegacyModels.Player;
using LegacyRound = LegacyDataImporter.LegacyModels.Round;
using Player = LegacyDataImporter.Models.Player;
using Round = LegacyDataImporter.Models.Round;
using System.Linq;
using LegacyDataImporter.Models;

namespace LegacyDataImporter
{
    public class Program
    {
        public static string StorageConnectionString
        {
            get
            {
                const string envVar = "StorageConnectionString";
                return Configuration[envVar];
            }
        }

        public static string DatabaseConnectionString
        {
            get
            {
                const string envVar = "DbConnectionString";

                return Configuration[envVar];
            }
        }

        public static bool IsDevelopment
        {
            get
            {
                bool isDevelopment;
                bool.TryParse(Environment.GetEnvironmentVariable("IsDevelopment"), out isDevelopment);

                return isDevelopment;
            }
        }

        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder();

            if (IsDevelopment)
            {
                Console.WriteLine("DEVELOPMENT");
                configuration.AddUserSecrets();
            }

            Configuration = configuration.Build();

            var program = new Program();
            program.Run();
        }

        private static IMapper _mapper;
        private static IMapper Mapper
        {
            get
            {
                Console.Write("Building map...");
                if (_mapper != null)
                {
                    return _mapper;
                }

                var config = new MapperConfiguration(ClassMaps.BuildMaps);

                var mapper = config.CreateMapper();
                Console.WriteLine("Done");
                return _mapper = mapper;
            }
        }

        const string ClubsTable = "clubs";
        const string RoundsTable = "rounds";
        const string PlayersTable = "players";

        private void Run()
        {
            if (string.IsNullOrEmpty(StorageConnectionString))
            {
                Console.WriteLine("No storage credentials");
                return;
            }

            var storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            var dbContext = new DdhpContext(DatabaseConnectionString);

            var importer = new Importer(tableClient, Mapper);

            importer.Import<Team, Club>(ClubsTable, dbContext.Teams);
            importer.Import<LegacyRound, Round>(RoundsTable, dbContext.Rounds);
            importer.Import<LegacyPlayer, Player>(PlayersTable, dbContext.Players.Include(q => q.CurrentAflTeam).ToList());

            Console.WriteLine("SUCCESS");
        }
    }
}
