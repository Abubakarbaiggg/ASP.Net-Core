using ASP.Net_Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.Net_Core.Controllers
{
    public class TestController : Controller
    {
        private readonly DataContext _context;
        public TestController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.courses = new SelectList(_context.Courses, "Id", "Title");
            return View();
        }
    }
}
