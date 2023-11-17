//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Linq;
using System;
using System.Security.Cryptography;
using StudentAttendanceTracker.Models.Identity;
using StudentAttendanceTracker.Models.Helpers;
using StudentAttendanceTracker.Models.DatabaseModels;
using StudentAttendanceTracker.Models.Initialization;
using StudentAttendanceTracker.Models.ViewModels;
using StudentAttendanceTracker.Models.ExcelModels;
using Microsoft.IdentityModel.Tokens;
using MethodTimer;

namespace StudentAttendanceTracker.Areas.Student.Controllers
{
    /// <summary>
    /// Controls site functioning for the Instructor section of the website
    /// </summary>
    [ResponseCache(NoStore = true, Duration = 0)]
    [Authorize(Roles = "Instructor")]
    [Area("Faculty")]
    public class InstructorController : Controller
    {
        private readonly AttendanceTrackerContext context;

        private readonly SignInManager<User> signInManager;
        /// <summary>
        /// InstructorController constructor to assign private AttendanceTrackerContext and SignInManager object
        /// </summary>
        /// <param name="ctx">AttendanceTrackerContext object</param>
        /// <param name="mngr">SignInManager object</param>
        public InstructorController(AttendanceTrackerContext ctx, SignInManager<User> mngr)
        {
            context = ctx;
            signInManager = mngr;
        }

        /// <summary>
        /// method for displaying the home page of the Instructor section
        /// </summary>
        /// <returns>faculty/Instructor/home.cshtml View</returns>
        public IActionResult Home()
        {

            Instructor instructor = context.Instructors.Include(x => x.Courses).First(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            

            return signInManager.IsSignedIn(User) ? View(instructor) : RedirectToAction("Index", "Home", new { area = "" });

        }

        /// <summary>
        /// Method for displaying the course page of the instructor section
        /// </summary>
        /// <param name="id">CourseId of the course to be access</param>
        /// <returns></returns>
        public IActionResult Course(int id)
        {

            Instructor Instructor = context.Instructors.First(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            Course course = context.Courses.Include(c=> c.Students).Where(c => c.InstructorId == Instructor.InstructorId).First(c => c.CourseId == id);
            AccessCode? history = context.AccessCodes.FirstOrDefault() != null ? context.AccessCodes.FirstOrDefault(a => a.CourseID == id) : null;
            ViewBag.AccessCode = history != null ? history.Code : "";
            return View(course);
        }

        /// <summary>
        /// Generates a new access code for a course
        /// </summary>
        /// <param name="id">CourseId of the course to generate an access code for</param>
        /// <returns>The course action</returns>
        public IActionResult GenerateCode(int id)
        {
            
            string? accessCode = context.AccessCodes.FirstOrDefault() != null ? context.AccessCodes.FirstOrDefault(a => a.CourseID == id)?.Code : null;
            if (accessCode is null)
            {
                accessCode = HelperMethods.GetRandomCharacters(6);
                context.AccessCodes.Add(new AccessCode { Code = accessCode, CourseID = id }); //change time to specified time
            }
            context.SaveChanges();
            return RedirectToAction("Course", "Instructor", new { id });
        }

        [HttpGet]
        public IActionResult Report(int id)
        {
            return View("~/Views/Shared/Report.cshtml",new FacultyReportViewModel { CourseId = id, Caller = "Instructor" });

        }

        
     

   

    
    }
}
