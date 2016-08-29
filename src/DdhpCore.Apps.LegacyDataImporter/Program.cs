using System;
using System.Linq;
using LegacyDataImporter.LegacyModels;
using LegacyDataImporter.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

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
                configuration.AddUserSecrets();
            }

            Configuration = configuration.Build();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("clubs");
            try
            {
                table.CreateIfNotExistsAsync().Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.Flatten().InnerException;
            }

            var dbContext = new DdhpContext(DatabaseConnectionString);

            ImportTeams(dbContext, table);
        }

        private static void ImportTeams(DdhpContext dbContext, CloudTable table)
        {
            var teams = dbContext.Teams;

            var clubs = teams.Select(team => MapTeamToClub(team));

            TableBatchOperation batchOperation = new TableBatchOperation();
            clubs.ToList().ForEach(club => batchOperation.InsertOrReplace(club));

            table.ExecuteBatchAsync(batchOperation).Wait();
        }

        private static Club MapTeamToClub(Team team)
        {
            return new Club
            {
                LegacyId = team.Id,
                CoachName = team.CoachName,
                ClubName = team.TeamName,
                Email = team.Email
            };
        }
    }
}
