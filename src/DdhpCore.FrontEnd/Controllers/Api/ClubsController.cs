using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DdhpCore.FrontEnd.Models.Api.Read;
using DdhpCore.FrontEnd.Models.Storage;
using DdhpCore.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [HttpGet("{year}/{clubId}")]
        public async Task<IActionResult> GetClubSeason(int year, string clubId)
        {
            var club = await _storage.Retrieve<ClubSeason>(clubId, year.ToString());

            if (club == null)
            {
                return NotFound();
            }

            return new ObjectResult(club);
        }

        [HttpGet]
        public async Task<IActionResult> GetClubs()
        {
            var clubs = await _storage.GetAllRows<Club>();

            return new ObjectResult(_mapper.Map<IEnumerable<Club>, IEnumerable<ClubApi>>(clubs));
        }
    }
}
