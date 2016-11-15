using AutoMapper;
using LegacyDataImporter.Extensions;
using LegacyDataImporter.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace LegacyDataImporter.Importers
{
    public abstract class Migrator
    {
        protected readonly CloudTableClient TableClient;

        protected Migrator(CloudTableClient tableClient)
        {
            TableClient = tableClient;
        }
        public abstract void Migrate();
    }

    public class PlayedTeamMigrator : Migrator
    {
        private readonly string _pickedTeamTableName;
        private readonly IMapper _mapper;

        public PlayedTeamMigrator(CloudTableClient tableClient, string pickedTeamTableName, IMapper mapper)
            : base(tableClient)
        {
            _mapper = mapper;
            _pickedTeamTableName = pickedTeamTableName;
        }

        public override void Migrate()
        {
            var table = TableClient.GetTableReference(_pickedTeamTableName);

            var pickedTeams = table.GetAllData<PickedTeam>();

            foreach (var pickedTeam in pickedTeams)
            {
                var playedTeam = new PlayedTeam();
                _mapper.Map(pickedTeam, playedTeam);


            }
        }
    }
}