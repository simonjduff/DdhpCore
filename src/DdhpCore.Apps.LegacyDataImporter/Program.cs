using System;
using System.Collections.Generic;
using AutoMapper;
using LegacyDataImporter.Importers;
using LegacyDataImporter.LegacyModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using LegacyContract = LegacyDataImporter.LegacyModels.Contract;
using LegacyFixture = LegacyDataImporter.LegacyModels.Fixture;
using LegacyPlayer = LegacyDataImporter.LegacyModels.Player;
using LegacyRound = LegacyDataImporter.LegacyModels.Round;
using LegacyStat = LegacyDataImporter.LegacyModels.Stat;
using Contract = LegacyDataImporter.Models.Contract;
using Fixture = LegacyDataImporter.Models.Fixture;
using Player = LegacyDataImporter.Models.Player;
using Round = LegacyDataImporter.Models.Round;
using Stat = LegacyDataImporter.Models.Stat;
using System.Linq;
using System.Linq.Expressions;
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
            else
            {
                configuration.AddEnvironmentVariables();
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
                if (_mapper != null)
                {
                    return _mapper;
                }

                Console.Write("Building map...");

                var config = new MapperConfiguration(ClassMaps.BuildMaps);

                var mapper = config.CreateMapper();
                Console.WriteLine("Done");
                return _mapper = mapper;
            }
        }

        const string ClubsTable = "clubs";
        const string RoundsTable = "rounds";
        const string PlayersTable = "players";
        const string ContractsTable = "contracts";
        const string PickedTeamsTable = "pickedTeams";
        const string PlayedTeamsTable = "playedTeams";
        const string FixturesTable = "fixtures";
        const string AflClubsTable = "aflclubs";
        const string StatsTable = "stats";

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

            var importerFactory = new ImporterFactory(tableClient, Mapper);

            var clubs = importerFactory
                .Importer<Team,Club>(ClubsTable)
                .Import(dbContext.Teams);
            importerFactory
                .Importer<LegacyRound, Round>(RoundsTable)
                .Import(dbContext.Rounds);
            importerFactory
                .Importer<AflTeam, AflClub>(AflClubsTable)
                .Import(dbContext.AflTeams);
            importerFactory
                .Importer<LegacyFixture, Fixture>(FixturesTable)
                .Import(dbContext.Fixtures);
            var players = importerFactory
                .Importer<LegacyPlayer, Player>(PlayersTable)
                .Import(dbContext.Players.Include(q => q.CurrentAflTeam));
            importerFactory
                .Importer<LegacyContract, Contract>(ContractsTable)
                .Import(dbContext.Contracts);
            importerFactory
                .Importer<RoundPlayer, PickedTeam>(PickedTeamsTable)
                .Mapper(PickedTeamMapper(clubs, players))
                .Import(dbContext.RoundPlayers
                    .Include(q => q.Player)
                    .Include(q => q.Contract) 
                    .Include(q => q.Round));

            var roundPlayers = from rp in dbContext.RoundPlayers
                join stat in dbContext.Stats
                on new {Round = rp.RoundId, PlayerId = rp.PlayerId}
                equals new {Round = stat.Round, PlayerId = stat.PlayerId}
                select new {RoundPlayer = rp, Stat = stat};

            foreach (var rp in roundPlayers.ToList())
            {
                rp.RoundPlayer.Stat = rp.Stat;
            }

           importerFactory
               .Importer<RoundPlayer, PlayedTeam>(PlayedTeamsTable)
               .Mapper(PlayedTeamMapper(clubs, players))
               .Import(roundPlayers.Select(q => q.RoundPlayer).AsQueryable()
                   .Include(q => q.Player)
                   .Include(q => q.Contract)
                   .Include(q => q.Round));

            var statRounds = dbContext.Stats.GroupBy(q => q.Round);

            importerFactory
                .Importer<LegacyStat, Stat>(StatsTable)
                .LogStart(() => statRounds.First().Key.ToString())
                .Import(statRounds.First().AsQueryable());

            foreach (var statRound in statRounds.Skip(1))
            {
                importerFactory
                .Importer<LegacyStat, Stat>(StatsTable)
                .LogStart(() => statRound.Key.ToString())
                .Import(statRound.AsQueryable(), false);
            }
            Console.WriteLine("SUCCESS");
        }

        private Func<IQueryable<RoundPlayer>, IEnumerable<PickedTeam>> PickedTeamMapper(IEnumerable<Club> clubs, IEnumerable<Player> players)
        {
            var clubsDictionary = clubs.ToDictionary(q => q.LegacyId);
            var playersDictionary = players.ToDictionary(q => q.LegacyId);

            return (roundPlayers) =>
            {
                var result = new List<PickedTeam>();

                var roundData = roundPlayers.GroupBy(q => q.RoundId);
                foreach (var round in roundData)
                {
                    var teamData = round.GroupBy(q => q.Contract.TeamId);

                    foreach (var team in teamData)
                    {
                        var teamPlayers = new List<PickedTeam.TeamPlayer>();

                        var pickedTeam = new PickedTeam
                        {
                            Round = round.Key,
                            ClubId = clubsDictionary[team.Key].Id,
                            Id = Guid.NewGuid(),
                            Team = new List<PickedTeam.TeamPlayer>()
                        };
                        teamPlayers.AddRange(
                            team.Select(
                                q =>
                                    new PickedTeam.TeamPlayer
                                    {
                                        PickedPosition = q.PickedPosition.Single(),
                                        PlayerId = playersDictionary[q.PlayerId].Id
                                    }));
                        pickedTeam.Team = teamPlayers;
                        result.Add(pickedTeam);
                    }
                }
                return result;
            };
        }

        private Func<IQueryable<RoundPlayer>,IEnumerable<PlayedTeam>> PlayedTeamMapper(IEnumerable<Club> clubs, IEnumerable<Player> players)
        {
            var clubsDictionary = clubs.ToDictionary(q => q.LegacyId);
            var playersDictionary = players.ToDictionary(q => q.LegacyId);

            return (roundPlayers) =>
            {
                var result = new List<PlayedTeam>();

                var roundData = roundPlayers.GroupBy(q => q.RoundId);
                foreach (var round in roundData)
                {
                    var teamData = round.GroupBy(q => q.Contract.TeamId);

                    foreach (var team in teamData)
                    {
                        var teamPlayers = new List<PlayedTeam.TeamPlayer>();

                        var pickedTeam = new PlayedTeam
                        {
                            Round = round.Key,
                            ClubId = clubsDictionary[team.Key].Id,
                            Id = Guid.NewGuid(),
                            Team = new List<PlayedTeam.TeamPlayer>()
                        };
                        teamPlayers.AddRange(
                            team.Select(
                                q =>
                                    new PlayedTeam.TeamPlayer
                                    {
                                        PickedPosition = q.PickedPosition.Single(),
                                        PlayerId = playersDictionary[q.PlayerId].Id,
                                        PlayedPosition = q.PlayedPosition.SingleOrDefault(),
                                        Stat = Mapper.Map<LegacyStat, Stat>(q.Stat),
                                        Score = GetScore(q.PlayedPosition.SingleOrDefault(), Mapper.Map<LegacyStat, Stat>(q.Stat))
                                    }));
                        pickedTeam.Team = teamPlayers;
                        pickedTeam.Score = pickedTeam.Team.Sum(q => q.Score);
                        result.Add(pickedTeam);
                    }
                }
                return result;
            };
        }

        private int GetScore(char? playedPosition, Stat stat)
        {
            if (!playedPosition.HasValue)
            {
                return 0;
            }

            switch (playedPosition)
            {
                case 'f':
                    return stat.Goals*6 + stat.Behinds;
                case 'm':
                    return stat.Disposals;
                case 'r':
                    return stat.Marks + stat.Hitouts;
                case 't':
                    return stat.Tackles*6;
                default:
                    return 0;
            }
        }
    }
}
