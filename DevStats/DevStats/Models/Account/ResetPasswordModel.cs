using System.ComponentModel.DataAnnotations;

namespace DevStats.Models.Account
{
    public class ResetPasswordModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string NewPassword { get; set; }
    }
}