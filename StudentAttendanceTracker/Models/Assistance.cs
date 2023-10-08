//C# and Razor code written by Zaid Abuisba

namespace StudentAttendanceTracker.Models;

public partial class Assistance
{
    /// <summary>
    /// Primary key for assistance table
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Date the issue was opened
    /// </summary>
    public DateTime Date { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Foreign key for the course assosciated with this object
    /// </summary>
    public int? CourseId { get; set; }
    /// <summary>
    /// Course object for the course associated with this object
    /// </summary>
    public Course? Course { get; set; }

    /// <summary>
    /// Foreign key for the course assosciated with this object
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Course object for the course associated with this object
    /// </summary>
    public Student? Student { get; set; }
}
