using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZDeals.Data.Entities
{
    [Table("DealCategory")]
    public class DealCategoryJoin
    {
        public int DealId { get; set; }
        public DealEntity Deal { get; set; }

        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
