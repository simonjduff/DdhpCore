using System;
using System.Net.Http;
using System.Threading.Tasks;
using DdhpCore.FrontEnd.Models.Values;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DdhpCore.FrontEnd.Extensions;
using DdhpCore.FrontEnd.Models.Api.Read;
using DdhpCore.FrontEnd.Models.Storage;

namespace DdhpCore.FrontEnd.Controllers
{
    [Route("[controller]")]
    public class ClubSeasonsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClubSeasonsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{year}/{name}")]
        public async Task<IActionResult> Index(int year, string name)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

            var clubResponse = await client.GetStringAsync($"/api/clubs/{name}");
            var club = JsonConvert.DeserializeObject<ClubApi>(clubResponse);

            var clubSeasonResponse = await client.GetStringAsync($"/api/clubs/{year}/{club.Id}");

            var deserialized = JsonConvert.DeserializeObject<ClubSeasonApi>(clubSeasonResponse);

            return View(deserialized);
        }
    }
}
