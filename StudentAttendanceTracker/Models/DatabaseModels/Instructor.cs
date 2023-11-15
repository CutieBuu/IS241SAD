//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using StudentAttendanceTracker.Models.Identity;
using System;
using System.Collections.Generic;

namespace StudentAttendanceTracker.Models.DatabaseModels
{
    /// <summary>
    /// Instructor class that corresponds to Instructors table in the database
    /// </summary>
    public class Instructor
    {
        public Instructor()
        {
            Courses = new HashSet<Course>();
        }

        /// <summary>
        /// Primary key for Instructor table
        /// </summary>
        public int InstructorId { get; set; }

        /// <summary>
        /// Instructor's first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Instructor's last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Instructor's email address 
        /// Should be the username of the corresponding user
        /// </summary>
        public string InstructorEmail { get; set; } = string.Empty;

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
        /// Collection of courses the Instructor teaches
        /// </summary>
        public virtual ICollection<Course> Courses { get; set; }
    }
}