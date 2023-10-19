//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceTracker.Models
{
    /// <summary>
    /// Admin class that corresponds to Admins table in the database
    /// </summary>
    public class Administrator
    {

        /// <summary>
        /// Primary key for Admin table
        /// </summary>
        public int AdministratorId { get; set; }

        /// <summary>
        /// Admins first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Admins last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Admins email address 
        /// Should be the username of the corresponding user
        /// </summary>
        public string AdministratorEmail { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key of the corresponding user object
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// User object associated with this Admin
        /// </summary>
        [ValidateNever]
        public User? User { get; set; }


    }
}