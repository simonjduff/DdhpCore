using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DdhpCore.FrontEnd.Models.Api.Read;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DdhpCore.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");
            var response = await client.GetStringAsync("/api/clubs");

            var clubs = JsonConvert.DeserializeObject<IEnumerable<ClubApi>>(response);

            return View(clubs);
        }
    }
}
