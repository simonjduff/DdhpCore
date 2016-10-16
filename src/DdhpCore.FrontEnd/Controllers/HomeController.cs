using Microsoft.AspNetCore.Mvc;

namespace DdhpCore.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
