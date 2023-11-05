//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; } = string.Empty;

       
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [ValidateNever]
        public string NewPassword { get; set; } = string.Empty;
    }
}
