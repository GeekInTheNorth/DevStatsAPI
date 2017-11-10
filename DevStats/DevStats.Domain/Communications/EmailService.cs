using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace DevStats.Domain.Communications
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(IdentityMessage message)
        {
            SendEmail(message.Destination, message.Subject, message.Body);

            return Task.CompletedTask;
        }

        public void SendEmail(string destination, string subject, string body)
        {
            var emailUserName = ConfigurationManager.AppSettings["EmailUserName"];
            var emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            var host = ConfigurationManager.AppSettings["EmailHost"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]);
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