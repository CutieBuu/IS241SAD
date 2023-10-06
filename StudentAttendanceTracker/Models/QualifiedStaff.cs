using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace StudentAttendanceTracker.Models
{

    public class QualifiedStaff
    {

        public int QualifiedStaffId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string QualifiedStaffEmail { get; set; } = string.Empty;


        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public User? User { get; set; }

    }
}