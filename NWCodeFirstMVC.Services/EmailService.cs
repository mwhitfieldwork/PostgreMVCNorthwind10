using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using NWCodeFirstMVC.Domain.Contracts;

namespace NWCodeFirstMVC.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("yourEmail@gmail.com", "yourAppPassword"),
                EnableSsl = true
            };

            var mail = new MailMessage("yourEmail@gmail.com", to, subject, body)
            {
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
