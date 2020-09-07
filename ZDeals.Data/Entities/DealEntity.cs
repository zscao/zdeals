using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("Deals")]
    public class DealEntity: EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Highlight { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal FullPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DealPrice { get; set; }
        
        [MaxLength(20)]
        public string Discount { get; set; }
        
        public DateTime? ExpiryDate { get; set; }

        [Required]
        [MaxLength(400)]
        public string Source { get; set; }

        public int? BrandId { get; set; }
        public BrandEntity Brand { get; set; }

        public int? StoreId { get; set; }

        public StoreEntity Store { get; set; }

        /// <summary>
        /// the file id of the default picture
        /// </summary>
        public string DefaultPicture { get; set; }

        public ICollection<DealCategoryJoin> DealCategory { get; set; }

        public ICollection<DealPictureEntity> Pictures { get; set; }

        public ICollection<VisitHistoryEntity> VisitHistory { get; set; }

        public DateTime CreatedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedTime { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedTime { get; set; }

        public bool Verified { get; set; }

        public DateTime? VerifiedTime { get; set; }

        [MaxLength(50)]
        public string VerifiedBy { get; set; }

        public bool FreeShipping { get; set; }

    }
}
