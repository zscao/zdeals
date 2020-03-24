using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities.Sales
{
    [Table("Deals")]
    public class DealEntity: EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Highlight { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal FullPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DealPrice { get; set; }
        
        [MaxLength(100)]
        public string Discount { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }
        
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(400)]
        public string Source { get; set; }

        public int? StoreId { get; set; }

        public StoreEntity Store { get; set; }

        public ICollection<DealCategoryJoin> DealCategory { get; set; }
    }
}
