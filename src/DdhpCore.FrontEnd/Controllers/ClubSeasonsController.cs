using System;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DdhpCore.FrontEnd.Models.Api.Read;
using DdhpCore.FrontEnd.Models.Values;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DdhpCore.FrontEnd.Extensions;

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
            var response = await _httpClient.ApiBaseAddress(Request).GetAsync($"clubs/{(Year)year}/{Uri.EscapeUriString(name)}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed {response.ReasonPhrase}");
            }

            var deserialized = JsonConvert.DeserializeObject<ClubSeason>(await response.Content.ReadAsStringAsync());

            return View(deserialized);
        }
    }
}
