using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using StudentAttendanceTracker.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models.ViewModels
{
    public class ReportViewModel
    {
        [ValidateNever]
        public List<Course>? Courses { get; set; } = null!;

        [ValidateNever]
        public string? Caller { get; set; } = null!;

        [Remote("UsernamesAreValid", "Validation", "")]
        public string? StudentUsernames { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a course")]
        [Remote("CourseIsSelected", "Validation", "")]
        public int CourseId { get; set; }

        [Remote("CheckDatesAreSequential", "Validation", "", AdditionalFields = "EndDate")]
        [DataType(DataType.Date, ErrorMessage = "Must be valid format (mm/dd/yy)")]
        public DateTime? StartDate { get; set; }

        [Remote("CheckDatesAreSequential", "Validation", "", AdditionalFields = "StartDate")]
        [DataType(DataType.Date, ErrorMessage = "Must be valid format (mm/dd/yy)")]
        public DateTime? EndDate { get; set; }
    }
}
