using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using StudentAttendanceTracker.Models.DatabaseModels;

namespace StudentAttendanceTracker.Models.ViewModels
{
    public class StudentReportViewModel
    {
        [ValidateNever]
        public Student Student { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [ValidateNever]
        public Course Course { get; set; }

        [ValidateNever]
        public List<Attendance> AttendanceLogs { get; set; }
    }
}
