using System;
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

        public string PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiry { get; set; }

        public int AccessAttempts { get; set; }

        public bool IsInRole(string role)
        {
            if (string.IsNullOrWhiteSpace(Role) || string.IsNullOrWhiteSpace(role)) return false;

            return Role.Equals(role, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}