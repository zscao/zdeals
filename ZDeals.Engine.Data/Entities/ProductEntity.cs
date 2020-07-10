using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Engine.Data.Entities
{
    [Table("Products")]
    public class ProductEntity: EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string HighLight { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal FullPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SalePrice { get; set; }

        [Required]
        [MaxLength(3)]
        public string PriceCurrency { get; set; }

        [MaxLength(50)]
        public string Manufacturer { get; set; }

        [MaxLength(20)]
        public string Brand { get; set; }

        [MaxLength(50)]
        public string Sku { get; set; }

        [MaxLength(50)]
        public string Mpn { get; set; }

        [MaxLength(100)]
        public string Store { get; set; }

        [MaxLength(400)]
        public string Url { get; set; }

        [Required]
        public DateTime ParsedTime { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedTime { get; set; }
    }
}
