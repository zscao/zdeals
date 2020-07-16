using Microsoft.EntityFrameworkCore;

using ZDeals.Engine.Data.Entities;

namespace ZDeals.Engine.Data
{
    public class EngineDbContext: DbContext
    {
        public EngineDbContext() { }

        public EngineDbContext(DbContextOptions<EngineDbContext> options) : base(options) { }


        public DbSet<ProductEntity> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
