using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities.Identity
{
    [Table("RefreshTokens")]
    public class RefreshTokenEntity: EntityBase
    {
        [Required]
        [MaxLength(200)]
        public string Token { get; set; }

        [Required]
        [MaxLength(100)]
        public string JwtId { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool Used { get; set; }

        public int UserId { get; set; }

        public UserEntity User { get; set; }

    }
}
