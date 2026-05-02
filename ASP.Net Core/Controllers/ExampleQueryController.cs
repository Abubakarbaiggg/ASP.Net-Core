using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_Core.Controllers
{
    public class ExampleQueryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetString(string name)
        {
            return View();
        }
    }
}
