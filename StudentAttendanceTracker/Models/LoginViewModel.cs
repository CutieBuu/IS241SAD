//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// The email address of the user account
        /// </summary>
        [Required(ErrorMessage = "Please enter an email address: ")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password of user. used to sign in with Signinmanager
        /// </summary>
        [Required(ErrorMessage = "Please enter a password: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Bool to remember login
        /// </summary>
        public bool RememberMe { get; set; } = true;
    }
}
