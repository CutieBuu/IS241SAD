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
    [Authorize(Roles = "Instructor,Admin,QualifiedStaff")]


    public class ReportController : Controller
    {
        private readonly AttendanceTrackerContext _context;

        public ReportController(AttendanceTrackerContext ctx) => _context = ctx;

        [HttpPost]
        public async Task<ActionResult> Report(FacultyReportViewModel model, string userType = "")
        {
            if (model.CourseId == 0)
            {
                //Change this
                model.Courses = _context.Courses.Include(c => c.Instructor)
                        .Include(c => c.Students)
                        .Where(c => c.Students.Count > 0 && c.Instructor != null)
                        .OrderBy(c => c.CourseId)
                        .ToList();
                ModelState.AddModelError("CourseId", "Please select a course");
                return userType switch
                {
                    "Instructor" => View("~/Areas/Faculty/Views/Instructor/Report.cshtml", model),
                    "QualifiedStaff" => View("~/Areas/Faculty/Views/QualifiedStaff/Report.cshtml", model),
                    "Admin" => View("~/Areas/Admin/Views/Admin/Report.cshtml", model),
                    _ => RedirectToAction("Index", "Home"),
                };
            }

            if (model.StartDate > model.EndDate)
            {
                //Change this
                model.Courses = _context.Courses.Include(c => c.Instructor)
                        .Include(c => c.Students)
                        .Where(c => c.Students.Count > 0 && c.Instructor != null)
                        .OrderBy(c => c.CourseId)
                        .ToList();
                ModelState.AddModelError("StartDate", "Start date must be before end date");
                   
            }
            
            List<Student> students = new();
            var course = _context.Courses.Include(c => c.Students).First(c => c.CourseId == model.CourseId);

            if (!model.StudentUsernames.IsNullOrEmpty())
            {

                var usernames = model.StudentUsernames.Replace(" ", "").Split(",");
                students = course.Students.Where(s => usernames.Contains(s.StudentEmail) && course.Students.Contains(s)).OrderBy(s => s.LastName).ToList();
                if (students.Count == 0)
                {
                    //change this
                    model.Courses = userType == "Instructor" ? null : _context.Courses.Include(c => c.Instructor)
                       .Include(c => c.Students)
                       .Where(c => c.Students.Count > 0 && c.Instructor != null)
                       .OrderBy(c => c.CourseId)
                       .ToList();
                    ModelState.AddModelError("StudentUsernames", "No students found with the specified usernames");

                    
                }


                foreach (string username in usernames)
                {
                    if (!students.Any(s => s.StudentEmail == username))
                    {
                        //Change this
                        model.Courses = model.Courses == null && userType == "Instructor" ? null : _context.Courses.Include(c => c.Instructor)
                       .Include(c => c.Students)
                       .Where(c => c.Students.Count > 0 && c.Instructor != null)
                       .OrderBy(c => c.CourseId)
                       .ToList();
                        ModelState.AddModelError("StudentUsernames", $"No student found with the username {username}");
                        
                    }
                }
            }
            else
            {
                students = course.Students.OrderBy(s => s.LastName).ToList();
            }

            if(ModelState.ErrorCount > 0)
            {
                return userType switch
                {
                    "Instructor" => View("~/Areas/Faculty/Views/Instructor/Report.cshtml", model),
                    "QualifiedStaff" => View("~/Areas/Faculty/Views/QualifiedStaff/Report.cshtml", model),
                    "Admin" => View("~/Areas/Admin/Views/Admin/Report.cshtml", model),
                    _ => RedirectToAction("Index", "Home"),
                };
            }



            StudentsInCourse reportModel = new()
            {
                Course = course,
                StudentAttendanceLogs = new(),
                StartDate = model.StartDate,
                EndDate = model.EndDate,

            };


            foreach (Models.DatabaseModels.Student s in students)
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
            string fileName = $"{course.CourseName}_Report.xlsx";
            System.IO.File.Delete(@$"./TemporaryReports/{result}.xlsx");
            ModelState.Clear();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
