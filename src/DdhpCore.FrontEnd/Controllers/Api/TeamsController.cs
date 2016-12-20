using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DdhpCore.FrontEnd.Extensions;
using DdhpCore.FrontEnd.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DdhpCore.FrontEnd.Controllers.Api
{
    [Route("api/[controller]")]
    public class TeamsController : Controller
    {
        private readonly CloudTable _table;
        private const string TableName = "playedTeams";

        public TeamsController(CloudTableClient tableClient)
        {
            _table = tableClient.GetTableReference(TableName);
        }

        [HttpGet("{round}/{clubId}")]
        public async Task<IActionResult> Get(int round, Guid clubId)
        {
            var query = TableOperation.Retrieve<PlayedTeam>(round.ToString(), clubId.ToString());
            var result = await _table.ExecuteAsync(query);

            return result.ToActionResult(this);
        }

        [HttpGet("{round}")]
        public async Task<IActionResult> Get(int round)
        {
            var query =
                new TableQuery<PlayedTeam>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, round.ToString()));

            TableContinuationToken continuation = null;
            var results = new List<PlayedTeam>(12);

            do
            {
                var result = await _table.ExecuteQuerySegmentedAsync(query, continuation);
                results.AddRange(result.Results);
                continuation = result.ContinuationToken;
            } while (continuation != null);

            if (!results.Any())
            {
                return NotFound();
            }

            return new ObjectResult(results);
        }
    }
}
