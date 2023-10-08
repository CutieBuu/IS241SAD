//C# and Razor code written by Zaid Abuisba
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace StudentAttendanceTracker.Models
{
    /// <summary>
    /// Course class that corresponds to courses table in the database
    /// </summary>
    public class Course
    {
        public Course()
        {
            Students = new HashSet<Student>();
        }

        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public int? ProfessorId { get; set; }

        public virtual Professor? Professor { get; set; }

        public virtual ICollection<Student>? Students { get; set; } = new List<Student>();
    }
}