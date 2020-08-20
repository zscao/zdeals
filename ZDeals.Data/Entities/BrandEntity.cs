using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("Brands")]
    public class BrandEntity: EntityBase
    {
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        public ICollection<DealEntity> Deals { get; set; }
    }
}
