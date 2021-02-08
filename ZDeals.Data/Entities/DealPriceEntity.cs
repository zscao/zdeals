using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("DealPrices")]
    public class DealPriceEntity: EntityBase
    {
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }


        public DateTime UpdatedTime { get; set; }

        [Required]
        public int DealId { get; set; }


        public DealEntity Deal { get; set; }

        [MaxLength(50)]
        public string PriceSource { get; set; }

    }
}
