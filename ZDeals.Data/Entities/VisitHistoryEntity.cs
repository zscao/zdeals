using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("VisitHistory")]
    public class VisitHistoryEntity: EntityBase
    {
        [Required]
        public int DealId { get; set; }

        [Required]
        public DateTime VisitedTime { get; set; }

        [MaxLength(200)]
        public string ClientIp { get; set; }

        [MaxLength(50)]
        public string SessionToken { get; set; }

        [MaxLength(50)]
        public string SessionId { get; set; }

        public DealEntity Deal { get; set; }
    }
}
