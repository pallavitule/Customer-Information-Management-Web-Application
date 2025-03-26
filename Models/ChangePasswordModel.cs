using System.ComponentModel.DataAnnotations;
namespace MVCDHProject5.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [Display(Name = "UserName")]
        [RegularExpression("[A-Za-z0-9-._@+]*")]
        public string Name { get; set; }
    }
}
