using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Engine.Data.Entities
{
    [Table("QueuedPages")]
    public class QueuedPageEntity: EntityBase
    {
        [Required]
        [MaxLength(400)]
        public string Url { get; set; }
        
        [MaxLength(400)]
        public string ParentUrl { get; set; }

        [Required]
        [MaxLength(100)]
        public string SiteCode { get; set; }

        [Required]
        public bool IsRetry { get; set; }

        public DateTime? LastRequest { get; set; }
    }
}
