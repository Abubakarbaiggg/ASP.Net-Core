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
using Microsoft.AspNetCore.Authorization;

namespace ASP.Net_Core.Controllers
{
    [Authorize(Roles = "Student")]
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

            var student = await _context.Students.Include(x => x.Enrollment).ThenInclude(c => c.Course)
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

            var student = await _context.Students
                .Include(s => s.Enrollment)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var selectedIds = student.Enrollment?.Select(x => x.CourseId).ToList() ?? new List<int>();
            var items = _context.Courses.Select(x => new SelectListItem
            {
                Text = x.Title,
                Value = x.Id.ToString(),
                Selected = selectedIds.Contains(x.Id)
            }).ToList();

            var vm = new CreateStudentViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Enrolled = student.Enrolled,
                Courses = items
            };

            return View(vm);
        }

        // POST: Student/Edit/5
        // Accept a view model so the Edit view can be re-rendered on validation errors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateStudentViewModel vm)
        {
            var student = _context.Students.Find(vm.Id);
            student.Name = vm.Name;
            student.Enrolled = vm.Enrolled;
            var studentById = await _context.Students.Include(x => x.Enrollment).FirstOrDefaultAsync(s => s.Id == id);
            var existingIds = studentById.Enrollment.Select(x => x.CourseId).ToList();
            var selectedIds = vm.Courses.Where(x => x.Selected).Select(x => int.Parse(x.Value)).ToList();
            var toAdd = selectedIds.Except(existingIds).ToList();
            var toRemove = existingIds.Except(selectedIds).ToList();
                student.Enrollment = student.Enrollment.Where(x => !toRemove.Contains(x.CourseId)).ToList();

            foreach (var item in toAdd)
            {
                student.Enrollment.Add(new StudentCourse
                {
                    CourseId = item
                });
            }
            _context.Students.Update(student);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
           
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
