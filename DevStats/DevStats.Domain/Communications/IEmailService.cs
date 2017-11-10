using Microsoft.AspNet.Identity;

namespace DevStats.Domain.Communications
{
    public interface IEmailService : IIdentityMessageService
    {
        void SendEmail(string destination, string subject, string body);
    }
}