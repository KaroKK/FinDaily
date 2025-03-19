using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinBackend.Api.Models;

namespace FinBackend.Models
{
       public class CashFlow
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string FlowDesc { get; set; }

            public decimal FlowAmount { get; set; }
            public DateTime FlowDate { get; set; }

            public string FlowType { get; set; }

            public int? CatId { get; set; }
            [ForeignKey("CatId")]
            public Categ? Category { get; set; }

            public int? PayId { get; set; }
            [ForeignKey("PayId")]
            public PayWay? PayWay { get; set; }

            public int UserId { get; set; }

            [Required]
            public DateTime CreatedOn { get; set; } = DateTime.UtcNow;  
        }
    }
