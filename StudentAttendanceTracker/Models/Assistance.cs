using System;
using System.Collections.Generic;

namespace StudentAttendanceTracker.Models;

public partial class Assistance
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.MinValue;

    public int? ClassId { get; set; }
    public Course? Class { get; set; }

    public int StudentId { get; set; }
    public Student? Student { get; set; }
}
