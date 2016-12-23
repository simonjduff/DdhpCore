using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DdhpCore.FrontEnd.Models.Api;
using DdhpCore.FrontEnd.Models.Values;
using DdhpCore.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace DdhpCore.FrontEnd.Controllers.Api
{
    [Route("api/[controller]")]
    public class ClubsController : Controller
    {
        private readonly ILogger<ClubsController> _logger;
        private readonly IStorageFacade _storage;
        private readonly IMapper _mapper;

        public ClubsController(ILoggerFactory loggerFactory,
            IStorageFacade storage,
            IMapper mapper)
        {
            _mapper = mapper;
            _storage = storage;
            _logger = loggerFactory.CreateLogger<ClubsController>();
        }

        [HttpGet]
        public async Task<IEnumerable<Club>> Get()
        {
            try
            {
                var query = new TableQuery<Storage.Models.Club>();

                var clubs = await _storage.BatchQuery(query);
                return _mapper.Map<IEnumerable<Storage.Models.Club>, IEnumerable<Club>>(clubs);
            }
            catch (Exception ex)
            {
                _logger.LogError((int)LogEventId.TableStorageQueryFailure, ex, "Failure getting clubs from storage");
                throw;
            }
        }

        [HttpGet("{name}")]
        public async Task<Club> Get(string name)
        {
            var club = await _storage.Retrieve<Storage.Models.Club>("ALL_CLUBS", name);

            return _mapper.Map<Storage.Models.Club, Club>(club);
        }

        [HttpGet("{name}/{round}")]
        public async Task<IActionResult> Get(string name, int round)
        {
            RoundValue roundValue = round;

            var club = await _storage.Retrieve<Storage.Models.Club>("ALL_CLUBS", name);
            var mapped = _mapper.Map<Storage.Models.Club, Club>(club);

            var contracts = await _storage.GetAllByPartition<Storage.Models.Contract>(club.Id.ToString());
            var filteredContracts = contracts.Where(q => q.FromRound <= roundValue && q.ToRound >= roundValue);
            var mappedContracts =
                _mapper.Map<IEnumerable<Storage.Models.Contract>, IEnumerable<Contract>>(filteredContracts);

            var playerIdGroups = filteredContracts.Select(c => c.PlayerId.ToString()).GroupBy(g => g.ToString().Substring(0, 1));

            var players = new List<Storage.Models.Player>();
            foreach (var group in playerIdGroups)
            {
                players.AddRange(await _storage.GetRowsInPartition<Storage.Models.Player>(group.Key, group));
            }

            var mappedPlayers = _mapper.Map<IEnumerable<Storage.Models.Player>, IEnumerable<Player>>(players).ToDictionary(q => q.Id);
            foreach (var contract in mappedContracts)
            {
                contract.Player = mappedPlayers[contract.PlayerId];
            }

            var playedTeam = await _storage.Retrieve<Storage.Models.PlayedTeam>(roundValue.ToString(), club.Id.ToString());

            mapped.ClubAtRound = new ClubAtRound
            {
                Contracts = mappedContracts,
                Round = roundValue,
                Team = _mapper.Map<Storage.Models.PlayedTeam, PlayedTeam>(playedTeam)
            };

            return new ObjectResult(mapped);
        }
    }
}
