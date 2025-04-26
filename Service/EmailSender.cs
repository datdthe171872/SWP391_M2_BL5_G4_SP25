using System.Net.Mail;
using System.Net;

namespace SWP391_M2_BL5_G4_SP25.Service
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var from = _config["EmailSettings:From"];
            var password = _config["EmailSettings:Password"];
            var host = _config["EmailSettings:Host"];
            var port = int.Parse(_config["EmailSettings:Port"]);

            var smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential(from, password)
            };

            using var message = new MailMessage(from, toEmail, subject, body);
            message.IsBodyHtml = true;
            await smtp.SendMailAsync(message);
        }
    }
}
