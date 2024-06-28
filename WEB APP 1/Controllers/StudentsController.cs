using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_APP_1.Data;
using WEB_APP_1.Models.Entities;
using WEB_APP_1.Models;


namespace WEB_APP_1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            var viewModel = new AddStudentsViewModel
            {
                Courses = _dbContext.Courses
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.CourseName
                    })
                    .ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddStudentsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Courses = _dbContext.Courses
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.CourseName
                    })
                    .ToList();
                return View(viewModel);
            }

            var student = new Student
            {
                Id = Guid.NewGuid(),
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                CourseId = viewModel.CourseId
            };

            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var students = await _dbContext.Students
                .Include(s => s.Course)
                .ToListAsync();
            return View(students);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new AddStudentsViewModel
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Phone = student.Phone,
                CourseId = student.CourseId,
                Courses = await _dbContext.Courses
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.CourseName
                    })
                    .ToListAsync()
            };

            ViewBag.StudentId = student.Id;

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AddStudentsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Courses = await _dbContext.Courses
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.CourseName
                    })
                    .ToListAsync();
                ViewBag.StudentId = id;
                return View(viewModel);
            }

            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.FirstName = viewModel.FirstName;
            student.LastName = viewModel.LastName;
            student.Email = viewModel.Email;
            student.Phone = viewModel.Phone;
            student.CourseId = viewModel.CourseId;

            _dbContext.Entry(student).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(List));
        }
        private bool StudentExists(Guid id)
        {
            return _dbContext.Students.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("List", "Students");
        }
    }
}
