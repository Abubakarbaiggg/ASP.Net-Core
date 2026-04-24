using ASP.Net_Core.Data;
using ASP.Net_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.Net_Core.Controllers
{
    public class CoursesController : Controller
    {
        private readonly DataContext _context;

        public CoursesController(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get Section Of Course Model
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }
        [HttpGet]
        public IActionResult Create() { 
            return View();
        }
        [HttpGet]
        public IActionResult Details(int Id) { 
            var course = _context.Courses.Where(c => c.Id == Id).FirstOrDefault();
            return View(course);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var course = _context.Courses.Where(c => c.Id == Id).FirstOrDefault();
            return View(course);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var course = _context.Courses.Where(c => c.Id == Id).FirstOrDefault();
            return View(course);
        }

        /// <summary>
        /// Post Section Of Course Model
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost]
        public IActionResult Create(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
