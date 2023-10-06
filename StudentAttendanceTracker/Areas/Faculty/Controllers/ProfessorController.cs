using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StudentAttendanceTracker.Areas.Student.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    [Authorize(Roles = "Professor")]
    [Area("Faculty")]
    public class ProfessorController : Controller
    {
        private AttendanceTrackerContext context;
        private SignInManager<User> signInManager;

        public ProfessorController(AttendanceTrackerContext ctx, SignInManager<User> mngr)
        {
            context = ctx;
            signInManager = mngr;
        }
       
        public IActionResult Home()
        {

            Professor professor = context.Professors.First(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.Courses = context.Courses.Where(p => p.ProfessorId == professor.ProfessorId).ToList();

            return signInManager.IsSignedIn(User) ? View(professor) : RedirectToAction("Index", "Home", new {area = ""});

		}



    }
}
