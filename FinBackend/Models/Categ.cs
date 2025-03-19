using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinBackend.Api.Models
{
    public class Categ
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string CatName { get; set; }
        
        public string CatInfo { get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public int UserId { get; set; }

        [ForeignKey("Id")]

        //TheUser` will not be loaded from the database until it is accessed.

        public virtual User TheUser { get; set; }
    }
}
