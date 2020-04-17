using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("Stores")]
    public class StoreEntity: EntityBase
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Website { get; set; }

        [MaxLength(100)]
        public string Domain { get; set; }

        public ICollection<DealEntity> Deals { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedTime { get; set; }
    }
}
