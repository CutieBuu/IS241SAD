﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceTracker.Models.Initialization;

namespace StudentAttendanceTracker.Controllers
{
    public class ValidationController : Controller
    {
        private readonly AttendanceTrackerContext _context;

        public ValidationController(AttendanceTrackerContext ctx) => _context = ctx;

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
                if(!_context.Students.Any(s => s.StudentEmail == username))
                {
                    return Json($"Username {username} does not exist");
                }
            }
            return Json(true);
        }


       
    }
}
