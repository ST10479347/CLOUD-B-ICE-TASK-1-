using cldv_act_2._1.Models;
using cldv_act_2._1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace cldv_act_2._1.Controllers
{
    public class HomeController : Controller
    {
        private readonly TableStorageService _storage;

        public HomeController(TableStorageService storage)
        {
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _storage.GetAllCoursesAsync();
            var students = await _storage.GetAllStudentsAsync();

            ViewBag.CourseCount = courses.Count;
            ViewBag.StudentCount = students.Count;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}