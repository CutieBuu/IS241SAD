using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace StudentAttendanceTracker.Models
{

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