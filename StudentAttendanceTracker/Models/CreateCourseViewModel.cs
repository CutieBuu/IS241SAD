//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Please enter a course name.")]
        public string? CourseName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter an instructor email.")]
        public string? InstructorEmail { get; set; } = null!;

  
        public DateTime CourseStartTime { get; set; }


        public DateTime CourseEndTime { get; set; }
    }
}
