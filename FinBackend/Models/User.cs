using System.ComponentModel.DataAnnotations;

namespace FinBackend.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string PassHash { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
