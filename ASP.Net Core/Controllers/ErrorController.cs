using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_Core.Controllers
{
    [Route("Error/{statusCode}")]
    public class ErrorController : Controller
    {
        public IActionResult Index(int statusCode)
        {
            switch (statusCode) {
                case 404:
                    ViewBag.ErrorMessage = "Page Not Found";
                    break;
                default:
                    ViewBag.ErrorMessage = "An unexpected error occurred";
                    break;
            }

            return View("NotFound");
        }
    }
}
