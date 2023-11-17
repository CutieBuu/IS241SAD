using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceTracker.Models.Initialization;

namespace StudentAttendanceTracker.Controllers
{
    public class ValidationController : Controller
    {
        private AttendanceTrackerContext _context;

        public ValidationController(AttendanceTrackerContext ctx) => _context = ctx;

        public JsonResult CheckDatesAreSequential(string StartDate, string EndDate)
        {
            if(StartDate.IsNullOrEmpty() || EndDate.IsNullOrEmpty())
            {
                return Json(true);
            }

            if (DateTime.TryParse(StartDate, out DateTime resultBefore) && DateTime.TryParse(EndDate, out DateTime resultAfter))
            {
                return resultBefore.Date < resultAfter.Date ? Json(true) : Json("Start date must be before end date");
            }
            return Json("Invalid date format must be (m/d/yyyy).");
            
        }

        public JsonResult CourseIsSelected(int CourseId)
        {
            if(CourseId == 0)
            {
                return Json("Please select a course");
            }
            return Json(true);
        }

        public JsonResult UsernamesAreValid(string StudentUsernames)
        {
            if(StudentUsernames.IsNullOrEmpty())
            {
                return Json(true);
            }

            string[] usernames = StudentUsernames.Replace(" ", "").Split(',');
            foreach(string username in usernames)
            {
                if(username.IsNullOrEmpty())
                {
                    return Json("Please enter a valid username");
                }
                if(!_context.Students.Any(s => s.StudentEmail == username))
                {
                    return Json($"Username {username} does not exist");
                }
            }
            return Json(true);
        }


       
    }
}
