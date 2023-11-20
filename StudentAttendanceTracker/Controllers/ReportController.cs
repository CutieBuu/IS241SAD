using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceTracker.Models.ExcelModels;
using StudentAttendanceTracker.Models.Helpers;
using StudentAttendanceTracker.Models.ViewModels;
using StudentAttendanceTracker.Models.DatabaseModels;
using StudentAttendanceTracker.Models.Initialization;
using System.Security.Claims;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace StudentAttendanceTracker.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]


    public class ReportController : Controller
    {
        private readonly AttendanceTrackerContext _context;

        public ReportController(AttendanceTrackerContext ctx) => _context = ctx;

        [HttpPost]
        public async Task<ActionResult> Report(ReportViewModel model)
        {


            if (model.StartDate > model.EndDate)
            {
                ModelState.AddModelError("StartDate", "Start date must be before end date");
            }

            List<Student> students = new();
            var usernames = model.StudentUsernames?.Replace(" ", "").Split(",");
            var course = _context.Courses.Include(c => c.Students).Include(c => c.Instructor).Include(c => c.Instructor.User).First(c => c.CourseId == model.CourseId);
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(course.Instructor.UserId != id && model.Caller == "Instructor")
            {
                TempData["ErrorMessage"] = $"You are not the instructor for {course.CourseName}";
                return RedirectToAction("Home", "Instructor", new {Area = "Faculty"});
            }
            

            if (model.Caller == "Student")
            {
                if(!course.Students.Any(s => s.UserId == id))
                {
                    TempData["ErrorMessage"] = $"You are not enrolled in {course.CourseName}";
                    return RedirectToAction("Home", "Student", new { Area = "Student" });
                }
                students.Add(_context.Students.First(s => s.StudentEmail == usernames[0]));
            }
            else
            {
                students = ValidateUsernames(model, students, usernames, course);
                if (students.Count == 0)
                {
                    ModelState.AddModelError("StudentUsernames", "No students found with those usernames");
                }
            }

            if(ModelState.IsValid == false)
            {
                model.Courses = _context.Courses.Include(c => c.Instructor)
                        .Include(c => c.Students)
                        .Where(c => c.Students.Count > 0 && c.Instructor != null)
                        .OrderBy(c => c.CourseId)
                        .ToList();
                return View("~/Views/Shared/Report.cshtml", model);
            }

            StudentsInCourse reportModel = new()
            {
                Course = course,
                StudentAttendanceLogs = new(),
                StartDate = model.StartDate,
                EndDate = model.EndDate,

            };


            foreach (Student s in students)
            {
                reportModel.StudentAttendanceLogs.Add(new StudentAttendance
                {
                    Student = s,
                    AttendanceLogs = _context.AttendanceLogs.Include(a => a.AccessCode).Where(a => a.StudentID == s.StudentId && a.AccessCode.CourseID == model.CourseId).ToList()
                });
            }


            ExcelHandler handler = new();
            var result = await handler.CreateExcelFileAsync(reportModel);

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(@$"./TemporaryReports/{result}.xlsx");
            string fileName;
            if (model.Caller != "Student")
            {
                fileName = $"{course.CourseName.Replace(" ","_")}_Report.xlsx";
            }
            else
            {
                fileName = $"{students[0].FirstName.FirstCharToUpper()}_{students[0].LastName.FirstCharToUpper()}_{course.CourseName.Replace(" ", "_")}_Report.xlsx";
            }
            System.IO.File.Delete(@$"./TemporaryReports/{result}.xlsx");
            ModelState.Clear();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private static List<Student> ValidateUsernames(ReportViewModel model, List<Student> students, string[] usernames, Course course)
        {
            if (model.StudentUsernames.IsNullOrEmpty())
            {
                students = course.Students.OrderBy(s => s.LastName).ToList();
            }
            else
            {
                students = course.Students.Where(s => usernames.Contains(s.StudentEmail) && course.Students.Contains(s)).OrderBy(s => s.LastName).ToList();
            }

            return students;
        }
    }
}
