using Microsoft.AspNetCore.Mvc;
using cldv_act_2._1.Models;
using cldv_act_2._1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cldv_act_2._1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly TableStorageService _storage;
        private readonly QueueService _queue;

        public StudentsController(TableStorageService storage, QueueService queue)
        {
            _storage = storage;
            _queue = queue;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _storage.GetAllStudentsAsync();
            return View(students);
        }

        public async Task<IActionResult> Details(string id)
        {
            var student = await _storage.GetStudentAsync(id);
            if (student == null) return NotFound();

            var courses = await _storage.GetAllCoursesAsync();
            ViewBag.AllCourses = courses; // for the enroll dropdown
            return View(student);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid) return View(student);
            await _storage.AddStudentAsync(student);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var student = await _storage.GetStudentAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (!ModelState.IsValid) return View(student);
            await _storage.UpdateStudentAsync(student);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var student = await _storage.GetStudentAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _storage.DeleteStudentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Sends an enrollment request onto the queue rather than
        // updating the student record directly.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(string studentId, string courseId)
        {
            var request = new EnrollmentRequest { StudentId = studentId, CourseId = courseId };
            await _queue.SendEnrollmentMessageAsync(request);

            TempData["Message"] = "Enrollment request submitted and queued for processing.";
            return RedirectToAction(nameof(Details), new { id = studentId });
        }

        // Manually triggers processing of everything currently on the queue.
       
        public async Task<IActionResult> ProcessQueue()
        {
            int count = await _queue.ProcessQueueAsync(_storage);
            TempData["Message"] = $"Processed {count} enrollment message(s) from the queue.";
            return RedirectToAction(nameof(Index));
        }
    }
}