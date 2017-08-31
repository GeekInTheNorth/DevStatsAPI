using Microsoft.AspNet.Identity;

namespace DevStats.Domain.Security
{
    public class ApplicationUser : IUser<int>
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string Role { get; set; }

        public string PasswordHash { get; set; }

        public int AccessAttempts { get; set; }
    }
}