using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Engine.Data.Entities
{
    [Table("VisitedPages")]
    public class VisitedPageEntity: EntityBase
    {
        [Required]
        [MaxLength(400)]
        public string Url { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContentType { get; set; }
        
        public DateTime LastVisitedTime { get; set; }
    }
}
