using System.Net;
using System.Net.Mail;

namespace Bank_Application.services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = _config.GetSection("SmtpSettings");

            var client = new SmtpClient
            {
                Host = smtp["Host"],
                Port = int.Parse(smtp["Port"]!),
                EnableSsl = bool.Parse(smtp["EnableSsl"]!),
                Credentials = new NetworkCredential(
                    smtp["SenderEmail"],
                    smtp["SenderPassword"]
                )
            };

            var mail = new MailMessage
            {
                From = new MailAddress(smtp["SenderEmail"]!),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);
        }
    }
}
