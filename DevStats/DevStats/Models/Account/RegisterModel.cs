using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevStats.Models.Account
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        public List<string> Roles { get; set; }

        public RegisterModel()
        {
            Roles = new List<string>
            {
                "User",
                "Admin"
            };
        }
    }
}