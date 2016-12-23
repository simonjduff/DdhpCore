using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DdhpCore.FrontEnd.Models.Api;
using DdhpCore.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace DdhpCore.FrontEnd.Controllers.Api
{
    [Route("api/[controller]")]
    public class FixturesController : Controller
    {
        private readonly ILogger<FixturesController> _logger;
        private readonly IMapper _mapper;
        private readonly IStorageFacade _storage;
        private const string TableName = "fixtures";

        public FixturesController(CloudTableClient tableClient,
            ILoggerFactory loggerFactory,
            IMapper mapper,
            IStorageFacade storage)
        {
            _storage = storage;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<FixturesController>();
        }

        [HttpGet("{round}")]
        public async Task<IActionResult> Get(int round)
        {
            TableQuery<DdhpCore.Storage.Models.Fixture> query = new TableQuery<Storage.Models.Fixture>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, round.ToString()));

            var fixtures = await _storage.BatchQuery(query);

            try
            {
                var postMap = _mapper.Map<IEnumerable<Storage.Models.Fixture>, IEnumerable<Fixture>>(fixtures);
                return new ObjectResult(postMap);
            }
            catch (Exception ex)
            {
                _logger.LogError((int)LogEventId.AutoMapperFailure, ex, "Failed to map fixtures");
                throw;
            }
        }
    }
}