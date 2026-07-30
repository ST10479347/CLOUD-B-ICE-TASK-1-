using cldv_act_2._1.Models;
using cldv_act_2._1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cldv_act_2._1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly TableStorageService _storage;

        public CoursesController(TableStorageService storage)
        {
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _storage.GetAllCoursesAsync();
            return View(courses);
        }

        public async Task<IActionResult> Details(string id)
        {
            var course = await _storage.GetCourseAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid) return View(course);
            await _storage.AddCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var course = await _storage.GetCourseAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Course course)
        {
            if (!ModelState.IsValid) return View(course);
            await _storage.UpdateCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var course = await _storage.GetCourseAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _storage.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}