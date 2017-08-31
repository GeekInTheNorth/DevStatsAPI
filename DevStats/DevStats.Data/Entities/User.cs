using System.ComponentModel.DataAnnotations;

namespace DevStats.Data.Entities
{
    public class User
    {
        public int ID { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string Role { get; set; }

        public string PasswordHash { get; set; }
    }
}