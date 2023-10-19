﻿using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceTracker.Models
{
    public class Attendance
    {
        /// <summary>
        /// Primary Key of the AttendanceLogs table
        /// </summary>
        public int AttendanceId { get; set; }
        
        /// <summary>
        /// ForeignKey of the AccessCodes table
        /// </summary>
        public string Code {  get; set; }

        /// <summary>
        /// AccessCode object that corresponds to the AccessCode foreign key
        /// </summary>
        public AccessCode AccessCode { get; set; }

        /// <summary>
        /// StudentID of the student who checked into their class
        /// </summary>
        public int StudentID { get; set; }

        /// <summary>
        /// Student object that corresponds to the StudentID foreign key
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        /// The time the student signed in
        /// </summary>
        public DateTime? SignInTime { get; set; }
    }
}
