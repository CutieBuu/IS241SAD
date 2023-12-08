//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models.ViewModels;
using System.Security.Claims;
using MailKit.Net.Smtp;
using MimeKit;
using StudentAttendanceTracker.Models.Initialization;
using System.Text;
using MimeKit.Utils;
using StudentAttendanceTracker.Models.Helpers;
using Microsoft.AspNetCore.Identity;
using StudentAttendanceTracker.Models.Identity;
namespace StudentAttendanceTracker.Controllers
{
    [Route("/[action]")]
    public class IssueController : Controller
    {


       
        public IActionResult Issue(string role)
        {
            
            if(!User.Identity.IsAuthenticated||!User.IsInRole(role))
                return RedirectToAction("Index", "Home", new { Area = "" });
            return View(new ReportIssueViewModel
            {
                Sender = User.FindFirstValue(ClaimTypes.Name),
                Caller = role
            }) ;
        }
        
        [HttpPost]
        public ActionResult Issue(ReportIssueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
         


            BodyBuilder bodyBuilder = new();

            var pathImage = "wwwroot/images/logo.png";
            var image = bodyBuilder.LinkedResources.Add(pathImage);
            image.ContentId = MimeUtils.GenerateMessageId();
           
            
            EmailSender emailSender = new();

            bodyBuilder.HtmlBody = BuildMessage(model.Description, model.Sender, image, "STLCC Logo");
            MimeMessage message = emailSender.BuildEmail(model.Sender, model.Recipient, model.Title, bodyBuilder);
            emailSender.SendEmail(message);
            TempData["SuccessMessage"] = $"Issue Reported to {model.Recipient}";
            return RedirectToAction("Home", model.Caller, new { Area = model.Caller == "Instructor" || model.Caller == "QualifiedStaff" ? "Faculty" : model.Caller });
        }

        [NonAction]
        private static string BuildMessage(string message, string sender, MimeEntity? image, string alt = "")
        {
            StringBuilder template = new();
            if (image != null)
                template.AppendLine($"<p><img src=\"cid:{image.ContentId}\" style=\"width: 20%; height: 10%;\" alt=\"{alt}\"></p>");
            template.AppendLine("<h2>This is an automated message from the STLCC Student Attendance Tracker website.</h2>\n\n\n");
            template.AppendLine($"<p style=\"font-size: 18px\">{message}</p>\n\n\n");
            template.AppendLine($"<h2>Do not reply to this email, if further communication is required, please reach out to this email address: <a href=\"mailto:{sender}\">{sender}</a></h2>");
            return template.ToString();
        }

    }
}
