using System.ComponentModel.DataAnnotations;

namespace DevStats.Models.Account
{
    public class ResetPasswordRequestModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}