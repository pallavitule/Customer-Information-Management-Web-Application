using System.ComponentModel.DataAnnotations;
namespace MVCDHProject5.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirmpasswordshouldmatchwithpassword.")]
        public string ConfirmPassword { get; set; }
    }
}
