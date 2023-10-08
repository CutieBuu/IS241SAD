//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;

namespace StudentAttendanceTracker.Areas.Student.Controllers
{
    /// <summary>
    /// Controls site functioning for the professor section of the website
    /// </summary>
    [ResponseCache(NoStore = true, Duration = 0)]
    [Authorize(Roles = "Professor")]
    [Area("Faculty")]
    public class ProfessorController : Controller
    {
        private readonly AttendanceTrackerContext context;

        private readonly SignInManager<User> signInManager;
        /// <summary>
        /// ProfessorController constructor to assign private AttendanceTrackerContext and SignInManager object
        /// </summary>
        /// <param name="ctx">AttendanceTrackerContext object</param>
        /// <param name="mngr">SignInManager object</param>
        public ProfessorController(AttendanceTrackerContext ctx, SignInManager<User> mngr)
        {
            context = ctx;
            signInManager = mngr;
        }

        /// <summary>
        /// method for displaying the home page of the professor section
        /// </summary>
        /// <returns>faculty/professor/home.cshtml View</returns>
        public IActionResult Home()
        {

            Professor professor = context.Professors.First(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Courses = context.Courses.Where(p => p.ProfessorId == professor.ProfessorId).ToList();

            return signInManager.IsSignedIn(User) ? View(professor) : RedirectToAction("Index", "Home", new { area = "" });

        }



    }
}
