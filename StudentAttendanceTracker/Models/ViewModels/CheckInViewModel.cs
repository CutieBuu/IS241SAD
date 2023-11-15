//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using StudentAttendanceTracker.Models.DatabaseModels;

namespace StudentAttendanceTracker.Models.ViewModels
{
    public class CheckInViewModel
    {
        public Course Course { get; set; }

        public string? AccessCode { get; set; } = "";

    }
}
