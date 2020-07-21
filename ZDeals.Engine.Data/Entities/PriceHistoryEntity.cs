using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Engine.Data.Entities
{
    [Table("PriceHisotry")]
    public class PriceHistoryEntity
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Sequence { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public virtual ProductEntity Product { get; set; }
    }
}
