using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceTracker.Models.Initialization;

namespace StudentAttendanceTracker.Controllers
{
    public class ValidationController(AttendanceTrackerContext ctx) : Controller
    {
        public JsonResult CheckDatesAreSequential(string StartDate, string EndDate)
        {
            if(StartDate.IsNullOrEmpty() || EndDate.IsNullOrEmpty())
            {
                return Json(true);
            }

            if (DateTime.TryParse(StartDate, out DateTime resultBefore) && DateTime.TryParse(EndDate, out DateTime resultAfter))
            {
                var d1 = new DateTime(resultBefore.Year, resultBefore.Month, resultBefore.Day, resultBefore.Hour, resultBefore.Minute, 0);
                var d2 = new DateTime(resultAfter.Year, resultAfter.Month, resultAfter.Day,resultAfter.Hour, resultAfter.Minute, 0);
                return d1 < d2 ? Json(true) : Json("Start date must be before end date");
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
                if(!ctx.Students.Any(s => s.StudentEmail == username))
                {
                    return Json($"Username {username} does not exist");
                }
            }
            return Json(true);
        }


        public JsonResult EmailIsValid(string? recipient)
        {
            if(recipient.IsNullOrEmpty())
            {
                return Json("Please enter an recipient address");
            }
            
            try
            {
                recipient = recipient.Replace(" ", "");
                 var addr = new System.Net.Mail.MailAddress(recipient);
                return addr.Address == recipient ? Json(true) : Json("Please enter a valid email address");
            }
            catch
            {
                return Json("Please enter a valid recipient address");
            }
        }

        public JsonResult TitleIsValid(string? title)
        {
            if(title.IsNullOrEmpty())
            {
                return Json("Please enter a title");
            }
            return Json(true);
        }

        public JsonResult DescriptionIsValid(string? description)
        {
            if (description.IsNullOrEmpty())
            {
                return Json("Please enter a description");
            }
            return Json(true);
        }


    }
}
