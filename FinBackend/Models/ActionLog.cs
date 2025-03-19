using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinBackend.Api.Models
{
    // Table for Saving user history, identifying who deleted a transaction and debugging 
    public class ActionLog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public string LogAction { get; set; }
        public string LogDetails { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public User TheUser { get; set; }
    }
}
