using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MVCDHProject5.Models
{
    public class Customer
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Custid { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "Varchar")]
        public string? Name { get; set; }

         [Column(TypeName = "Money")]
        public decimal? Balance { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "Varchar")]
        public string? City { get; set; }
        public bool Status { get; set; }
        
        public string State { get; set; }
        
        public string Country { get; set; }
        public string Continent { get; set; } = "Unknown";
    }
}
