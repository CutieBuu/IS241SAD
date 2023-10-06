using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceTracker.Models
{

    public class Student
    {
        public Student()
        {
            Courses = new HashSet<Course>();
        }
        public int StudentId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string StudentEmail { get; set; } = string.Empty;


        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public User? User { get; set; }


        public virtual ICollection<Course>? Courses { get; set; } = new List<Course>();
    }
}