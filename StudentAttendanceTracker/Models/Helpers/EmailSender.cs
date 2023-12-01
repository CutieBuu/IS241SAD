using Microsoft.AspNetCore.Mvc;
using StudentAttendanceTracker.Models.ViewModels;
using System.Security.Claims;
using MailKit.Net.Smtp;
using MimeKit;
using StudentAttendanceTracker.Models.Initialization;
using System.Text;
using MimeKit.Utils;
using Microsoft.AspNetCore.Identity;

namespace StudentAttendanceTracker.Models.Helpers
{
    public class EmailSender
    {
        

        public void SendEmail(MimeMessage message)
        {
            var client = new SmtpClient();
            client.Connect(host: "smtp.gmail.com", port: 465, useSsl: true);
            client.Authenticate(userName: "AttendanceTracker.DoNotReply@gmail.com", password: "REPLACE_THIS");
            client.Send(message);
            client.Disconnect(quit: true);
            client.Dispose();
        }

        public MimeMessage BuildEmail(string from, string to,string subject, BodyBuilder body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(name: from, address: "AttendanceTracker.DoNotReply@gmail.com"));
            message.To.Add(new MailboxAddress(name: to, address: to));
            message.Subject = subject;
            message.Body = body.ToMessageBody();
            return message;
        }

       
    }
}
