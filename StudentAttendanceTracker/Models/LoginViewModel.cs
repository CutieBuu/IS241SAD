using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter an email address: ")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Please enter a password: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        //public string ReturnUrl { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = true;
    }
}
