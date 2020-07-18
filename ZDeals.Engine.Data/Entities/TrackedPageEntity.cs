using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Engine.Data.Entities
{
    [Table("TrackedPages")]
    public class TrackedPageEntity: EntityBase
    {
        [Required]
        [MaxLength(400)]
        public string Url { get; set; }

        [MaxLength(100)]
        public string Store { get; set; }

        [Required]
        public DateTime AddedDate { get; set; }
    }
}
