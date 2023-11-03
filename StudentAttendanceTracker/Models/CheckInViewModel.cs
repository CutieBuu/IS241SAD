//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
namespace StudentAttendanceTracker.Models
{
    public class CheckInViewModel
    {
        public Course Course { get; set; }

        public string? AccessCode { get; set; } = "";

    }
}
