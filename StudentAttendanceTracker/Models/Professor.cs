using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace StudentAttendanceTracker.Models
{

    public class Professor
    {
        public Professor()
        {
            Classes = new HashSet<Course>();
        }

        public int ProfessorId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string ProfessorEmail { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public User? User { get; set; }

        public virtual ICollection<Course> Classes { get; set; }
    }
}