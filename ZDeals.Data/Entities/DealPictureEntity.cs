using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDeals.Data.Entities
{
    [Table("DealPictures")]
    public class DealPictureEntity
    {
        [Required]
        [MaxLength(100)]
        public string FileName { get; set; }
        [MaxLength(200)]
        public string Alt { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        
        public int DealId { get; set; }
        public DealEntity Deal { get; set; }
    }
}
