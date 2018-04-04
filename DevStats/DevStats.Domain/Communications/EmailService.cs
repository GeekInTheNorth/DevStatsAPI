using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DevStats.Domain.SystemProperties;
using Microsoft.AspNet.Identity;

namespace DevStats.Domain.Communications
{
    public class EmailService : IEmailService
    {
        private readonly ISystemPropertyRepository systemPropertyRepository;

        public EmailService(ISystemPropertyRepository systemPropertyRepository)
        {
            if (systemPropertyRepository == null) throw new ArgumentNullException(nameof(systemPropertyRepository));

            this.systemPropertyRepository = systemPropertyRepository;
        }

        public Task SendAsync(IdentityMessage message)
        {
            SendEmail(message.Destination, message.Subject, message.Body);

            return Task.CompletedTask;
        }

        public void SendEmail(string destination, string subject, string body)
        {
            var emailUserName = systemPropertyRepository.GetNonNullValue(SystemPropertyName.EmailUserName);
            var emailPassword = systemPropertyRepository.GetNonNullValue(SystemPropertyName.EmailPassword);
            var host = systemPropertyRepository.GetNonNullValue(SystemPropertyName.EmailHost);
            var port = Convert.ToInt32(systemPropertyRepository.GetNonNullValue(SystemPropertyName.EmailPort));

            var email = new MailMessage(new MailAddress(emailUserName, "(Do Not Reply)"), new MailAddress(destination))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailUserName, emailPassword);
                client.Timeout = 90000;

                client.Send(email);
            }
        }
    }
}