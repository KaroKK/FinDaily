using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinBackend.Api.Models
{
    public class PayWay
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string PayLabel { get; set; }

        public string? PayInfo { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? TheUser { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
