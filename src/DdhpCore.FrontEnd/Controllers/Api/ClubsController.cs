using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DdhpCore.FrontEnd.Extensions;
using DdhpCore.FrontEnd.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace DdhpCore.FrontEnd.Controllers.Api
{
    [Route("api/[controller]")]
    public class ClubsController : Controller
    {
        public ClubsController(CloudTableClient tableClient,
            ILoggerFactory loggerFactory)
        {
            _table = tableClient.GetTableReference(TableName);
            _logger = loggerFactory.CreateLogger<ClubsController>();
        }

        private readonly CloudTable _table;
        private readonly ILogger<ClubsController> _logger;
        private const string TableName = "clubs";

        // GET: api/clubs
        [HttpGet]
        public async Task<IEnumerable<Club>> Get()
        {
            try
            {
                var query = new TableQuery<Club>();

                return await query.BatchQuery<Club>(_table);
            }
            catch (Exception ex)
            {
                _logger.LogError((int)LogEventId.TableStorageQueryFailure, ex, "Failure getting clubs from storage");
                throw;
            }
        }

        // GET api/clubs/5
        [HttpGet("{name}")]
        public async Task<Club> Get(string name)
        {

            var query = TableOperation.Retrieve<Club>("ALL_CLUBS", name);
            var result = await _table.ExecuteAsync(query);

            return result.Result as Club;
        }
    }
}
