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
        public async Task<IEnumerable<ClubSeason>> Get()
        {
            try
            {
                var query = new TableQuery<ClubSeason>();

                var clubs = await _storage.BatchQuery(query);
                return clubs;
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

        [HttpGet("{year}/{name}")]
        public async Task<IActionResult> Get(int year, string name)
        {
            var club = await _storage.Retrieve<ClubSeason>(year.ToString(), name);

            return new ObjectResult(club);
        }
    }
}
