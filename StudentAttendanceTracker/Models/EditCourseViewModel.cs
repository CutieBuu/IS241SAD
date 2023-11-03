//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace StudentAttendanceTracker.Models
{
    public class EditCourseViewModel
    {
        [ValidateNever]
        public Course Course { get; set; } = null!;
     
        

        [ValidateNever]
        public Student Student { get; set; } = null!;
    }
}
