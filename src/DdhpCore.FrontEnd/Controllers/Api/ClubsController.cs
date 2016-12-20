using System.Collections.Generic;
using System.Threading.Tasks;
using DdhpCore.FrontEnd.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;

namespace DdhpCore.FrontEnd.Controllers.Api
{
    [Route("api/[controller]")]
    public class ClubsController : Controller
    {
        public ClubsController(CloudTableClient tableClient)
        {
            _table = tableClient.GetTableReference(TableName);
        }

        private readonly CloudTable _table;
        private const string TableName = "clubs";

        // GET: api/clubs
        [HttpGet]
        public async Task<IEnumerable<Club>> Get()
        {
            var query = new TableQuery<Club>();
            TableContinuationToken continuation = null;

            var results = new List<Club>();

            do
            {
                var result = await _table.ExecuteQuerySegmentedAsync(query, continuation);

                results.AddRange(result.Results);

                continuation = result.ContinuationToken;
            } while (continuation != null);

            return results;
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
