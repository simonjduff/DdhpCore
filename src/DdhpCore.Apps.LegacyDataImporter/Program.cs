using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
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

            var teams = dbContext.Teams;

            Console.WriteLine(teams.Count());
        }
    }
}
