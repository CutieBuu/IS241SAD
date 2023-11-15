//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models.ViewModels
{
    /// <summary>
    /// This class may be deleted. Currently used for easily creating new users in the development build.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Email address of the user
        /// </summary>

        [Required(ErrorMessage = "Please enter a valid email address")]
        [StringLength(255)]
        public string Username { get; set; } = string.Empty;


        /// <summary>
        /// Password of the user
        /// </summary>
        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        //[Compare("ConfirmPassword")]
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

        [Required(ErrorMessage = "Please select a role.")]
        public string? Role { get; set; } = null!;


        [ValidateNever]
        public IEnumerable<IdentityRole> Roles { get; set; } = null!;

        //        /// <summary>
        //        /// Password re-enter field to see if the password was typed in correctly twice
        //        /// </summary>
        //        [Required(ErrorMessage = "Please confirm your password.")]
        //        [DataType(DataType.Password)]
        //        [Display(Name = "Confirm Password")]
        //        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
