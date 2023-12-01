//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 https://github.com/vgc12
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceTracker.Models.DatabaseModels;
using StudentAttendanceTracker.Models.Initialization;
using StudentAttendanceTracker.Models.ViewModels;
using System.Diagnostics;
using System.Linq;
namespace StudentAttendanceTracker.Areas.Admin.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly AttendanceTrackerContext _context;

        public CourseController(AttendanceTrackerContext context) => _context = context;

        [HttpGet]
        public IActionResult Withdraw(int studentId, int courseId)
        {
             _context.Courses.Include(c => c.Students).First(c => c.CourseId == courseId).Students!.Remove(_context.Students.First(s => s.StudentId == studentId));
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Course Updated Successfully";
            return RedirectToAction("Edit", "Course", new { id = courseId });
        }

        [HttpGet]
        public IActionResult Course() => View(_context.Courses.Include(c => c.Instructor).ToList());

        [HttpGet]
        public IActionResult Create() => View(new CreateCourseViewModel());

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = new EditCourseViewModel
            {
                Course =
                _context.Courses.Include(c => c.Instructor)
                .Include(c => c.Students)
                .First(c => c.CourseId == id),

                Student = new Models.DatabaseModels.Student()
            };
            TempData["CourseName"] = model.Course.CourseName;
            ViewBag.Students = model.Course.Students!.ToList();
            return View(model);

        }

        
        public IActionResult Delete(int id)
        {
            _context.Courses.Remove(_context.Courses.First(c => c.CourseId == id));
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Course Deleted Successfully";
            return RedirectToAction("Course");
        }

        [HttpPost]
        public IActionResult Create(CreateCourseViewModel model)
        {
           
            Instructor? instructor = _context.Instructors.Include(i => i.Courses).FirstOrDefault(i => i.InstructorEmail == model.InstructorEmail);
            if (instructor == null && !model.InstructorEmail.IsNullOrEmpty())
            {
                ModelState.AddModelError("InstructorEmail", "Instructor does not exist");


            }
            if (model.CourseStartTime >= model.CourseEndTime)
            {
                ModelState.AddModelError("CourseStartTime", "Course Start Time must be before Course End Time");

            }
            model.CourseStartTime = ChangeDate(model.CourseStartTime);
            model.CourseEndTime = ChangeDate(model.CourseEndTime);
            var tempcourse = instructor?.Courses.FirstOrDefault(c => ((model.CourseStartTime >= c.CourseStartTime.AddMinutes(-14) && model.CourseStartTime <= c.CourseEndTime.AddMinutes(14))
                            || (model.CourseEndTime >= c.CourseStartTime.AddMinutes(-14) && model.CourseEndTime <= c.CourseEndTime.AddMinutes(14))));
            if (tempcourse != null)
            {
                ModelState.AddModelError("CourseStartTime", $"Instructor is already teaching a course between {tempcourse.CourseStartTime:t} and {tempcourse.CourseEndTime:t}");

            }
            //fix here
            if (model.CourseName == null)
            {
                ModelState.AddModelError("CourseName", "Must enter a course name");
            }
            if (ModelState.ErrorCount > 0)
            {
                return View(model);
            }
            
            Course course = new()
            {
                InstructorId = instructor != null ? instructor.InstructorId : null,
                CourseName = model.CourseName!,
                CourseStartTime = ChangeDate(model.CourseStartTime),
                CourseEndTime = ChangeDate(model.CourseEndTime)
            };

            _context.Courses.Add(course);
            _context.SaveChanges();




            return RedirectToAction("Edit", "Course", new { id = tempcourse == null ? _context.Courses.First(c => c.CourseId == _context.Courses.Count()).CourseId : tempcourse.CourseId });
        }

        [HttpPost]
        public IActionResult UpdateCourse(EditCourseViewModel model)
        {
            ModelState.Clear();

            if (_context.Courses.Include(c => c.Instructor).Any(c => c.Instructor.InstructorEmail == model.Course.Instructor!.InstructorEmail
            && c.CourseName == model.Course.CourseName && c.CourseStartTime.TimeOfDay == model.Course.CourseStartTime.TimeOfDay && c.CourseEndTime.TimeOfDay == model.Course.CourseEndTime.TimeOfDay))
            {
                ModelState.AddModelError("Course", "Please enter changes to the course.");
                TempData["CourseName"] = _context.Courses.First(c => c.CourseId == model.Course.CourseId).CourseName;
                ViewBag.Students = _context.Courses.Include(c => c.Students).First(c => c.CourseId == model.Course.CourseId).Students!.ToList();
                return View("Edit", model);
            }
            var instructor = _context.Instructors.FirstOrDefault(i => i.InstructorEmail == model.Course.Instructor!.InstructorEmail);

            //model.CourseDisplayName = coursePreChanges.CourseName;
            if (model.Course.CourseStartTime >= model.Course.CourseEndTime)
            {
                ModelState.AddModelError("Course.CourseStartTime", "Course Start Time must be before Course End Time");

            }
            model.Course.CourseStartTime = ChangeDate(model.Course.CourseStartTime);
            model.Course.CourseEndTime = ChangeDate(model.Course.CourseEndTime);

            var tempcourse = instructor != null
                            ? _context.Courses.Where(c => c.InstructorId == instructor.InstructorId)
                            .FirstOrDefault(c => c.CourseId != model.Course.CourseId && (
                            (model.Course.CourseStartTime >= c.CourseStartTime.AddMinutes(-14) && model.Course.CourseStartTime <= c.CourseEndTime.AddMinutes(14)) ||
                            (model.Course.CourseEndTime >= c.CourseStartTime.AddMinutes(-14) && model.Course.CourseEndTime <= c.CourseEndTime.AddMinutes(14))
                            ))
                            : null;
            if (tempcourse != null)
            {
                ModelState.AddModelError("Course.CourseStartTime", $"Instructor is already teaching a course between {tempcourse.CourseStartTime:t} and {tempcourse.CourseEndTime:t}. Please allow 15 minutes between courses.");
               
            }
            
            if (instructor == null && model.Course.Instructor.InstructorEmail != null)
            {
                ModelState.AddModelError("Course.Instructor.InstructorEmail", "Instructor email does not exist");

            }

            if (model.Course.CourseName == null)
            {
                ModelState.AddModelError("Course.CourseName", "Course Name is required");
            }


            if (ModelState.ErrorCount > 0)
            {
                TempData["CourseName"] = _context.Courses.First(c => c.CourseId == model.Course.CourseId).CourseName;
                ViewBag.Students = _context.Courses.Include(c => c.Students).First(c => c.CourseId == model.Course.CourseId).Students!.ToList();
                return View("Edit", model);
            }

            model.Course.CourseStartTime = ChangeDate(model.Course.CourseStartTime);
            model.Course.CourseEndTime = ChangeDate(model.Course.CourseEndTime);
            model.Course.Instructor = instructor;

            model.Course.InstructorId = instructor?.InstructorId;
            _context.Courses.Update(model.Course);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Course Updated Successfully";
            return RedirectToAction("Edit", "Course", new { id = model.Course.CourseId });

        }

        [NonAction]
        private static DateTime ChangeDate(DateTime date) => new(1, 1, 1, date.Hour, date.Minute, date.Second);
        //private bool ValidCourse

        [HttpPost]
        public IActionResult AddStudent(EditCourseViewModel model)
        {
            ModelState.Clear();
            model.Course = _context.Courses.Include(c => c.Instructor).Include(c => c.Students).First(c => c.CourseId == model.Course.CourseId);
            ViewBag.Students = model.Course.Students!.ToList();
            if (model.Student.StudentEmail != null && model.Student.FirstName != null && model.Student.LastName != null)
            {
                Models.DatabaseModels.Student student = _context.Students.Include(s => s.Courses).FirstOrDefault(s => s.FirstName == model.Student.FirstName && s.LastName == model.Student.LastName && s.StudentEmail == model.Student.StudentEmail)
                    ?? new Models.DatabaseModels.Student();
                if (student.StudentId == 0 || student.Courses == null)
                {
                    ModelState.AddModelError("Student", $"\"{model.Student.FirstName} {model.Student.LastName} {model.Student.StudentEmail}\" does not exist");


                    return View("Edit", model);
                }
                else if (student.Courses.Any(c => c.CourseId == model.Course.CourseId))
                {
                    ModelState.AddModelError("Student", $"\"{model.Student.FirstName} {model.Student.LastName}\" is already enrolled in this course");
                    return View("Edit", model);
                }
                else
                {

                    student.Courses.Add(model.Course);
                    _context.Students.Update(student);

                    _context.SaveChanges();
                }
            }
            else
            {

                if (model.Student.StudentEmail == null)
                {
                    ModelState.AddModelError("Student.StudentEmail", "Student Email is required");
                }
                if (model.Student.FirstName == null)
                {
                    ModelState.AddModelError("Student.FirstName", "First Name is required");
                }
                if (model.Student.LastName == null)
                {
                    ModelState.AddModelError("Student.LastName", "Last Name is required");
                }
                return View("Edit", model);
            }
            return RedirectToAction("Edit", "Course", new { id = model.Course.CourseId });
        }

       
       
    }
}
