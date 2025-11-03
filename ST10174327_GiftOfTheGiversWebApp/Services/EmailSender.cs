using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // In a real application, you would send the email here
            // For development, we'll just log the email to the console
            System.Console.WriteLine($"Email to: {email}, Subject: {subject}, Message: {htmlMessage}");
            
            // Uncomment and configure the following code to send real emails
            /*
            var client = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            
            mailMessage.To.Add(email);
            
            return client.SendMailAsync(mailMessage);
            */
            
            return Task.CompletedTask;
        }
    }

    public class EmailSettings
    {
        public string MailServer { get; set; } = "smtp.gmail.com";
        public int MailPort { get; set; } = 587;
        public string SenderName { get; set; } = "Gift of the Givers";
        public string SenderEmail { get; set; } = "noreply@giftofthegivers.org";
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; } = true;
    }
}
