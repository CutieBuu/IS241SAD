using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;

namespace StudentAttendanceTracker.Areas.Student.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    [Route("[area]/{action}")]
    
    [Authorize(Roles = "Student")]
    [Area("Student")]
    public class StudentController : Controller
    {
        private AttendanceTrackerContext context;
        public StudentController(AttendanceTrackerContext ctx) => context = ctx;

       
        public IActionResult Home()
        {
            Models.Student student = context.Students.First(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Courses = context.Courses.Where(x => x.Students.Contains(student)).ToList();
            return View(student);
        }

    }
}
