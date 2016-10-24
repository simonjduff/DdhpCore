using System;
using System.Linq;
using AutoMapper;
using LegacyDataImporter.Importers;
using LegacyDataImporter.LegacyModels;
using LegacyDataImporter.Models;
using LegacyDataImporter.Writers;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using LegacyRound = LegacyDataImporter.LegacyModels.Round;
using Round = LegacyDataImporter.Models.Round;

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

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<LegacyRound, Round>();
                    cfg.CreateMap<Team, Club>()
                        .ConvertUsing(team => new Club
                                        {
                                            LegacyId = team.Id,
                                            CoachName = team.CoachName,
                                            ClubName = team.TeamName,
                                            Email = team.Email,
                                            Id = Guid.NewGuid()
                                        });
                });

                var mapper = config.CreateMapper();
                Console.WriteLine("Done");
                return _mapper = mapper;
            }
        }

        const string ClubsTable = "clubs";
        const string RoundsTable = "rounds";

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

            Console.WriteLine("SUCCESS");
        }
    }
}
