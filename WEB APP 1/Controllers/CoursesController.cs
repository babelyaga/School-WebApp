using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_APP_1.Data;
using WEB_APP_1.Models.Entities;
using WEB_APP_1.Models;

namespace WEB_APP_1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CoursesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(AddCoursesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var course = new Course
            {
                CourseName = viewModel.CourseName,
                Subject = viewModel.Subject,
                Date = viewModel.Date,
                Location = viewModel.Location,
                IsActive = viewModel.IsActive
            };

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ListCourse");
        }

        [HttpGet]
        public async Task<IActionResult> ListCourse()
        {
            var courses = await _dbContext.Courses.ToListAsync();
            return View(courses);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCourse(Guid id)
        {
            var course = await _dbContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(Guid id, Course viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            _dbContext.Entry(viewModel).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Courses.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("ListCourse");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var course = await _dbContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ListCourse", "Courses");
        }
    }
}
