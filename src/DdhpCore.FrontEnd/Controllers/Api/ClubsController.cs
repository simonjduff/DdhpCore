using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DdhpCore.FrontEnd.Controllers.Api
{
    [Route("api/[controller]")]
    public class ClubsController : Controller
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public ClubsController(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            try
            {
                var table = await _dynamoDb.DescribeTableAsync("test");
            }
            catch (ResourceNotFoundException)
            {
                return new string[] {"Result: ", "Table not found"};
            }

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("healthcheck")]
        public JsonResult HealthCheck()
        {
            return Json(new {Result = "Ok"});
        }
    }
}
