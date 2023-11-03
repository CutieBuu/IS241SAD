//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace StudentAttendanceTracker.Models
{
    /// <summary>
    /// Qualified Staff class that corresponds to the qualified staff table in the database
    /// </summary>
    public class QualifiedStaff
    {
        /// <summary>
        /// Primary key for qualified staff table
        /// </summary>
        public int QualifiedStaffId { get; set; }

        /// <summary>
        /// Qualified staff member's first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Qualified staff member's last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Qualified staff member's email
        /// </summary>
        public string QualifiedStaffEmail { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key of the corresponding user object
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// User object associated with this qualified staff memeber
        /// </summary>
        [ValidateNever]
        public User? User { get; set; }

    }
}