using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("ActionHistory")]
    public class ActionHistoryEntity: EntityBase
    {
        [Required]
        public int DealId { get; set; }

        public DealEntity Deal { get; set; }

        [Required]
        [MaxLength(20)]
        public string Action { get; set; }

        [MaxLength(50)]
        public string ActedBy { get; set; }

        [Required]
        public DateTime ActedOn { get; set; }

        [MaxLength(200)]
        public string Notes { get; set; }
    }
}
