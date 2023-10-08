//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceTracker.Models
{
    /// <summary>
    /// User class that inherits from the IdentityUser class. allows for adding more columns to the AspNetUsers table.
    /// </summary>
    public class User : IdentityUser
    {
        [NotMapped]
        public IList<string> RoleNames { get; set; } = null!;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
    
}
