using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinBackend.Api.Models
{
    public class PlanBud
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int CatId { get; set; }
        [Required]
        public decimal LimitAmt { get; set; }
        [Required]
        public string PeriodTxt { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public User TheUser { get; set; }
        [ForeignKey("CatId")]
        public Categ TheCat { get; set; }
    }
}
