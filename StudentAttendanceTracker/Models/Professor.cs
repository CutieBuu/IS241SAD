//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace StudentAttendanceTracker.Models
{
    /// <summary>
    /// Professor class that corresponds to professors table in the database
    /// </summary>
    public class Professor
    {
        public Professor()
        {
            Classes = new HashSet<Course>();
        }

        /// <summary>
        /// Primary key for professor table
        /// </summary>
        public int ProfessorId { get; set; }

        /// <summary>
        /// Professor's first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Professor's last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Professor's email address 
        /// Should be the username of the corresponding user
        /// </summary>
        public string ProfessorEmail { get; set; } = string.Empty;

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
        /// Collection of courses the professor teaches
        /// </summary>
        public virtual ICollection<Course> Classes { get; set; }
    }
}