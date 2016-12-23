using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DdhpCore.FrontEnd.Configuration;
using DdhpCore.FrontEnd.Models.Api;
using DdhpCore.FrontEnd.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DdhpCore.FrontEnd.Controllers
{
    [Route("results")]
    public class ResultsController : Controller
    {
        private readonly IOptions<ApiOptions> _apiOptions;
        private readonly ILogger<ResultsController> _logger;

        public ResultsController(ILoggerFactory loggerFactory,
            IOptions<ApiOptions> apiOptions)
        {
            _apiOptions = apiOptions;
            _logger = loggerFactory.CreateLogger<ResultsController>();
        }

        [Route("index/{round}")]
        public async Task<IActionResult> Index(int round)
        {
            var viewModel = new ResultsViewModel
            {
                Round = round
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiOptions.Value.ApiRoot);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage fixturesResponse = await client.GetAsync($"api/fixtures/{round}");
                if (fixturesResponse.IsSuccessStatusCode)
                {
                    viewModel.Fixtures = await fixturesResponse.Content.ReadAsAsync<IEnumerable<Fixture>>();
                }

                HttpResponseMessage teamsResponse = await client.GetAsync($"api/teams/{round}");
                if (fixturesResponse.IsSuccessStatusCode)
                {
                    var playedTeams = await teamsResponse.Content.ReadAsAsync<IEnumerable<PlayedTeam>>();
                    viewModel.PlayedTeams = playedTeams.ToDictionary(team => team.ClubId);
                }
            }

            return View(viewModel);
        }
    }
}
