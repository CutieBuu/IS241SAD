using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity;

namespace StudentAttendanceTracker.Areas.Student.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    [Authorize(Roles = "QualifiedStaff")]
    [Area("Faculty")]
    public class QualifiedStaffController : Controller
    {
        private AttendanceTrackerContext context;


        public QualifiedStaffController(AttendanceTrackerContext ctx) => context = ctx;

       
        public IActionResult Home()
        {
            QualifiedStaff qualifiedStaff = context.QualifiedStaff.First(q => q.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(qualifiedStaff);
        }

    }
}
