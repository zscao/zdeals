using Microsoft.EntityFrameworkCore;

using ZDeals.Engine.Data.Entities;

namespace ZDeals.Engine.Data
{
    public class EngineDbContext: DbContext
    {
        public EngineDbContext() { }

        public EngineDbContext(DbContextOptions<EngineDbContext> options) : base(options) { }


        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<QueuedPageEntity> QueuedPages { get; set; }
        public DbSet<VisitedPageEntity> VisitedPages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VisitedPageEntity>(e =>
            {
                e.HasIndex(p => p.UrlHash).IsUnique(false);
            });
        }
    }
}
