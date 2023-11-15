using StudentAttendanceTracker.Models.DatabaseModels;
namespace StudentAttendanceTracker.Models.ExcelModels
{
    public class StudentsInCourse
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<StudentAttendance> StudentAttendanceLogs { get; set; }

        public Course Course { get; set; }
    }
}
