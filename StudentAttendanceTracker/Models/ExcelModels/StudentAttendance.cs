//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
using StudentAttendanceTracker.Models.DatabaseModels;

namespace StudentAttendanceTracker.Models.ExcelModels
{
    public class StudentAttendance
    {
        public Student Student { get; set; }

        public List<Attendance> AttendanceLogs { get; set; }
    }
}
