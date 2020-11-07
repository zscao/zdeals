using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("DealPriceHistory")]
    public class DealPriceHistoryEntity: EntityBase
    {
        [Required]
        public int DealId { get; set; }

        public int Sequence { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "date")]
        public DateTime UpdatedDate { get; set; }

        public DateTime UpdatedTime { get; set; }

        public DealEntity Deal { get; set; }
    }
}
