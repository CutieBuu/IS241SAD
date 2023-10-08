//C# and Razor code written by Zaid Abuisba
using Microsoft.AspNetCore.Identity;


namespace StudentAttendanceTracker.Models
{
    /// <summary>
    /// Class to pass multiple users to the admin page.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// List of all users
        /// </summary>
        public IEnumerable<User> Users { get; set; } = null!;
        /// <summary>
        /// List of all roles
        /// </summary>
        public IEnumerable<IdentityRole> Roles { get; set; } = null!;
    }
}
