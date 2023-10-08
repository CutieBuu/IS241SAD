//C# and Razor code written by Zaid Abuisba
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models
{
    /// <summary>
    /// This class may be deleted. Currently used for easily creating new users in the development build.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Email address of the user
        /// </summary>
        [Required(ErrorMessage = "Please enter an email")]
        [StringLength(255)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Password of the user
        /// </summary>
        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// First name of the user
        /// </summary>
        [Required(ErrorMessage = "Please enter a first name.")]
		[StringLength(255)]
		public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Last name of the user
        /// </summary>
        [Required(ErrorMessage = "Please enter a last name.")]
		[StringLength(255)]
		public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Password re-enter field to see if the password was typed in correctly twice
        /// </summary>
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
