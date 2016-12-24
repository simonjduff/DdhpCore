using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DdhpCore.FrontEnd.Models.Api;
using DdhpCore.FrontEnd.Models.Values;
using DdhpCore.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DdhpCore.FrontEnd.Controllers.Api
{
    [Route("api/[controller]")]
    public class RoundsController : Controller
    {
        private readonly ILogger<RoundsController> _logger;
        private readonly IStorageFacade _storage;
        private readonly IMapper _mapper;

        public RoundsController(ILoggerFactory loggerFactory,
            IStorageFacade storage,
            IMapper mapper)
        {
            _mapper = mapper;
            _storage = storage;
            _logger = loggerFactory.CreateLogger<RoundsController>();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            RoundValue roundId = id;

            var round = await _storage.Retrieve<Storage.Models.Round>(roundId.Year.ToString(), roundId.RoundNumber.ToString());

            return Ok(_mapper.Map<Storage.Models.Round, Round>(round));
        }
    }
}
