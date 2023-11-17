//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models.DatabaseModels;
using StudentAttendanceTracker.Models.Initialization;
using StudentAttendanceTracker.Models.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceTracker.Models.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Diagnostics;
using StudentAttendanceTracker.Models.ExcelModels;

namespace StudentAttendanceTracker.Areas.Student.Controllers
{
	/// <summary>
	/// Controls site functioning for the qualified staff section of the website
	/// </summary>
	[ResponseCache(NoStore = true, Duration = 0)]
	[Authorize(Roles = "QualifiedStaff")]
	[Area("Faculty")]
	public class QualifiedStaffController : Controller
	{
		private readonly AttendanceTrackerContext context;

		/// <summary>
		/// QualifiedStaffController constructor to assign private AttendanceTrackerContext object
		/// </summary>
		/// <param name="ctx">AttendanceTrackerContext object</param>
		public QualifiedStaffController(AttendanceTrackerContext ctx) => context = ctx;

		/// <summary>
		/// method for displaying the home page of the qualifiedstaff section
		/// </summary>
		/// <returns>faculty/qualifiedstaff/home.cshtml View</returns>
		public IActionResult Home()
		{
			QualifiedStaff qualifiedStaff = context.QualifiedStaff.First(q => q.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
			return View(qualifiedStaff);
		}


		[HttpGet]
		public ViewResult Report()
		{
            return View("~/Views/Shared/Report.cshtml",new ReportViewModel
            {
                Courses = context.Courses.Include(c => c.Instructor)
                        .Include(c => c.Students)
                        .Where(c => c.Students.Count > 0 && c.Instructor != null)
                        .OrderBy(c => c.CourseId)
                        .ToList(),
				Caller = "QualifiedStaff"
            });

		}

        
	
        
    }
}
