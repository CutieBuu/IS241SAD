//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Home() => View(GetStudent());

        public IActionResult Course(int id)
        {
            var student = GetStudent();
            string? accessCode = context.AttendanceLogs.Where(x => x.StudentID == student.StudentId)?.Include(x => x.AccessCode).FirstOrDefault(x => x.AccessCode.CourseID == id)?.Code;
            return View(new CheckInViewModel { Course = context.Courses.First(x => x.CourseId == id), AccessCode = accessCode });
        }
        

        /// <summary>
        /// Checks a user in for the class
        /// </summary>
        /// <param name="model">Model that contains information about the Access Code and Course</param>
        /// <returns>User back to the "Course" action</returns>
        [HttpPost]
        public IActionResult CheckIn(CheckInViewModel model)
        {
            Models.Student student = GetStudent();
            Course course = student.Courses!.First(x => x.CourseId == model.Course.CourseId);

            if (context.AccessCodes.Where(x => x.Code == model.AccessCode).Any(x => x.CourseID == model.Course.CourseId))
            {

                DateTime now = new(1, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                DateTime courseExpiration = course.CourseStartTime.AddMinutes(2);
                if (CheckedInToday(model, student))
                {
                    TempData["ErrorMessage"] = "You have already logged your attendance for this class today.";
                }
                else if (course.CourseStartTime < now && now < course.CourseEndTime)
                {
                    
                    Attendance attendance = new()
                    {
                        Code = model.AccessCode!,
                        AccessCode = context.AccessCodes.First(x => x.Code == model.AccessCode),
                        Student = student,
                        SignInTime = DateTime.Now,
                        StudentID = student.StudentId,
                        Tardy = now > courseExpiration
                    };
                    
                    context.AttendanceLogs.Add(attendance);
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Successfully Logged Attendance" ;
                    TempData["ErrorMessage"] = attendance.Tardy ? "You have been marked as late" : string.Empty;
                    
                
                }
                else
                {
                    TempData["ErrorMessage"] = "Please try again at " + course.CourseStartTime.ToString("hh:mm tt");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Access Code";
            }


            return RedirectToAction("Course", "Student", new { id = model.Course.CourseId });
        }

        private bool CheckedInToday(CheckInViewModel model, Models.Student student) =>
            context.AttendanceLogs.Where(x => x.Code == model.AccessCode).Where(x => x.StudentID == student.StudentId).Any(x => x.SignInTime.Value.Date == DateTime.Now.Date);

        /// <summary>
        /// Gets the student object from the database that corresponds to the currently logged in user.
        /// </summary>
        /// <returns>A Student Object</returns>
        public Models.Student GetStudent() => context.Students.Include(x => x.Courses).First(x => x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
