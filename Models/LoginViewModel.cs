using System.ComponentModel.DataAnnotations;
namespace MVCDHProject5.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        [RegularExpression("[A-Za-z0-9-._@+]*")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; } = "";
    }
}
