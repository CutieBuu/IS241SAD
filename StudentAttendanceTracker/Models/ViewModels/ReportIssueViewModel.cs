//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceTracker.Models.ViewModels
{
    public class ReportIssueViewModel
    {
        [Length(3, 500, ErrorMessage = "Title must be between 3 and 500 characters")]
        [Remote("TitleIsValid", "Validation", "")]
        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; } = string.Empty;

        [Length(10, 15000, ErrorMessage = "Description must be between 10 and 15000 characters")]
        [Remote("DescriptionIsValid", "Validation", "")]
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; } = string.Empty;

        public string? Sender { get; set; } = string.Empty;
        
        [Remote("EmailIsValid", "Validation", "")]
        [Required(ErrorMessage = "Please enter an email address")]
        [Length(3, 255, ErrorMessage = "Email address must be between 3 and 255 characters")]
        public string Recipient { get; set; } = string.Empty;

        public string? Caller { get; set; } = string.Empty;
    }
}
