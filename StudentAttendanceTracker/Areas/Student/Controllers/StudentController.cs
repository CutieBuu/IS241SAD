//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;

namespace StudentAttendanceTracker.Areas.Student.Controllers
{
    /// <summary>
    /// Controls site functioning for the student section of the website
    /// </summary>
    [ResponseCache(NoStore = true, Duration = 0)]
    [Route("[area]/{action}")]
    [Authorize(Roles = "Student")]
    [Area("Student")]
    public class StudentController : Controller
    {
        private readonly AttendanceTrackerContext context;
        /// <summary>
        /// StudentController constructor to assign private AttendanceTrackerContext object
        /// </summary>
        /// <param name="ctx">AttendanceTrackerContext object</param>
        public StudentController(AttendanceTrackerContext ctx) => context = ctx;

        /// <summary>
        /// method for displaying the home page of the student section
        /// </summary>
        /// <returns>/student/home.cshtml View</returns>
        public IActionResult Home()
        {
            Models.Student student = context.Students.First(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Courses = context.Courses.Where(x => x.Students!.Contains(student)).ToList();
            return View(student);
        }

    }
}
