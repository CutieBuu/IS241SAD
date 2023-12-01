﻿//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12

namespace StudentAttendanceTracker.Models.DatabaseModels;

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
    public string UserId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;




}
