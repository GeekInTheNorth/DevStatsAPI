using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace DevStats.Domain.Security
{
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        private readonly IApplicationUserStore<ApplicationUser, int> store;

        public ApplicationUserManager(IApplicationUserStore<ApplicationUser, int> store) : base(store)
        {
            this.store = store;
            UserTokenProvider = new EmailTokenProvider<ApplicationUser, int>();
            EmailService = new EmailService();
        }

        public async Task<bool> IsInRoleAsync(string userName, string role)
        {
            var user = await store.FindByNameAsync(userName);

            if (user == null) return false;

            return user.Role.Equals(role, StringComparison.CurrentCultureIgnoreCase);
        }

        public override Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return store.FindByEmailAsync(email);
        }

        public async Task GeneratePasswordResetEmail(ApplicationUser user, string rootUrl)
        {
            var token = await GenerateEmailConfirmationTokenAsync(user.Id);
            await store.SetSecurityStampAsync(user, token);

            var subject = "DevStats: Password Reset";
            var body = "Click here to reset your password: <a href='{0}/Account/ResetPassword?emailAddress={1}&token={2}' target='_blank'>Reset</a>";
            body = string.Format(body, rootUrl, HttpUtility.UrlEncode(user.EmailAddress), HttpUtility.UrlEncode(token));

            await SendEmailAsync(user.Id, subject, body);
        }

        public async Task<List<string>> ProcessPasswordReset(string emailAddress, string token, string password)
        {
            var errors = new List<string>();

            var user = await store.FindByEmailAsync(emailAddress);
            if (user != null)
            {
                var expirey = user.PasswordResetTokenExpiry ?? DateTime.MinValue;
                var userToken = user.PasswordResetToken ?? string.Empty;

                if (expirey < DateTime.Now) errors.Add("Token Expired");
                if (!userToken.Equals(token)) errors.Add("Invalid Token");

                if (!errors.Any())
                {
                    user.PasswordHash = PasswordHasher.HashPassword(password);
                    user.PasswordResetToken = null;
                    user.PasswordResetTokenExpiry = null;

                    try
                    {
                        await store.UpdateAsync(user);
                    }
                    catch (Exception ex)
                    {
                        errors.Add("Unknown Error");
                    }
                }
            }
            else
                errors.Add("Unrecognised User");

            return errors;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var emailUserName = ConfigurationManager.AppSettings["EmailUserName"];
            var emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            var host = ConfigurationManager.AppSettings["EmailHost"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]);
            var email = new MailMessage(new MailAddress(emailUserName, "(Do Not Reply)"), new MailAddress(message.Destination))
            {
                Subject = message.Subject,
                Body = message.Body,
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

            return Task.CompletedTask;
        }
    }
}