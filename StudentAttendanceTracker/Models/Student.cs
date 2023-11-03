//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceTracker.Models
{
    /// <summary>
    /// Student class that corresponds to students table in the database
    /// </summary>
    public class Student
    {

        public Student()
        {
            Courses = new HashSet<Course>();
        }

        /// <summary>
        /// Primary key for student table
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Students first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Students last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Students email address 
        /// Should be the username of the corresponding user
        /// </summary>
        public string StudentEmail { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key of the corresponding user object
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// User object associated with this student
        /// </summary>
        [ValidateNever]
        public User? User { get; set; }

        /// <summary>
        /// Collection of courses the student is enrolled in
        /// </summary>
        public virtual ICollection<Course>? Courses { get; set; } = new List<Course>();
    }
}