//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using StudentAttendanceTracker.Models.ViewModels;
using StudentAttendanceTracker.Models.Identity;
using StudentAttendanceTracker.Models.Initialization;
using Microsoft.EntityFrameworkCore;

namespace StudentAttendanceTracker.Areas.Admin.Controllers
{
    /// <summary>
    /// Controls site functioning for the Admin section of the website
    /// </summary>
    /// <remarks>
    /// StudentController constructor to assign private AttendanceTrackerContext object
    /// </remarks>
    /// <param name="userMngr">UserManager object</param>
    /// <param name="roleMngr">RoleManager object</param>
    [ResponseCache(NoStore = true, Duration = 0)]
    [Route("[area]/{action}")]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController(UserManager<User> userMngr, RoleManager<IdentityRole> roleMngr, AttendanceTrackerContext context) : Controller
    {

        /// <summary>
        /// Displays the home page of the admin section of the site
        /// </summary>
        /// <returns>Admin/Admin/Home</returns>
        public IActionResult Home()
        {
            return View(context.Admins.First(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        [HttpGet]
        public ViewResult Report()
        {
            return View("~/Views/Shared/Report.cshtml", new ReportViewModel
            {
                Courses = context.Courses.Include(c => c.Instructor)
                        .Include(c => c.Students)
                        .Where(c => c.Students.Count > 0 && c.Instructor != null)
                        .OrderBy(c => c.CourseId)
                        .ToList(),
                Caller = "Admin"
            }) ; 

        }


    }
}
