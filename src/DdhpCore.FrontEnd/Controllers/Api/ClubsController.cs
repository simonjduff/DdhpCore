using System.Threading.Tasks;
using AutoMapper;
using DdhpCore.FrontEnd.Models.Api.Read;
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

        [HttpGet("{year}/{name}")]
        public async Task<IActionResult> Get(int year, string name)
        {
            var club = await _storage.Retrieve<ClubSeason>(year.ToString(), name);

            if (club == null)
            {
                return NotFound();
            }

            return new ObjectResult(club);
        }
    }
}
