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
        public int UrlHash { get; set; }

        [Required]
        public DateTime LastVisitedTime { get; set; }
    }
}
