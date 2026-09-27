using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);

        Task SendEmailAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName);
    }
}
