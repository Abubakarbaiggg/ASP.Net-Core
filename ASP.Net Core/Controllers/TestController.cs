using ASP.Net_Core.Data;
using ASP.Net_Core.Models;
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

        public IActionResult Index2()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            CookieOptions cookies = new CookieOptions();
            cookies.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("Email",emp.Email, cookies);
            ViewBag.Saved = "Cookie Saved";
            return View();
        }
        public IActionResult ReadCookies()
        {
            ViewBag.Email = Request.Cookies["Email"].ToString();
            return View("Create");
        }
    }
}
