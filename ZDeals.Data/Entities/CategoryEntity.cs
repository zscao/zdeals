using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("Categories")]
    public class CategoryEntity: EntityBase
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        public int? ParentId { get; set; }

        public CategoryEntity Parent { get; set; }

        public ICollection<CategoryEntity> Children { get; set; }

        public ICollection<DealCategoryJoin> DealCategory { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedTime { get; set; }
    }
}
