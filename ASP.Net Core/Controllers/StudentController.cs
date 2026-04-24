using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.Net_Core.Data;
using ASP.Net_Core.Models;
using ASP.Net_Core.ViewModels;

namespace ASP.Net_Core.Controllers
{
    public class StudentController : Controller
    {
        private readonly DataContext _context;

        public StudentController(DataContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            var courses = _context.Courses.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString(),
            }).ToList();
            CreateStudentViewModel vm = new CreateStudentViewModel();
            vm.Courses = courses;
            return View(vm);
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    Name = vm.Name,
                    Enrolled = vm.Enrolled
                };

                _context.Add(student);
                await _context.SaveChangesAsync();

                if (vm.Courses != null)
                {
                    foreach (var c in vm.Courses.Where(x => x.Selected))
                    {
                        if (int.TryParse(c.Value, out int courseId))
                        {
                            _context.StudentCourses.Add(new StudentCourse
                            {
                                StudentId = student.Id,
                                CourseId = courseId
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // If we got here, repopulate the courses list so the view can render them again
            var courseEntities = _context.Courses.ToList();
            var allCourses = courseEntities.Select(x =>
            {
                var selected = false;
                if (vm.Courses != null)
                {
                    var match = vm.Courses.FirstOrDefault(cc => cc.Value == x.Id.ToString());
                    selected = match != null && match.Selected;
                }
                return new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Id.ToString(),
                    Selected = selected
                };
            }).ToList();
            vm.Courses = allCourses;

            return View(vm);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Enrolled")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
