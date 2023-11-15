using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using StudentAttendanceTracker.Models.DatabaseModels;

namespace StudentAttendanceTracker.Models.ViewModels
{
    public class FacultyReportViewModel
    {
        [ValidateNever]
        public List<Course>? Courses { get; set; } = null!;

        public string? StudentUsernames { get; set; }

        public int CourseId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
