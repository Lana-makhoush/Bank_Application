
using System.Net;
using System.Net.Mail;
namespace Bank_Application.services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}