using Microsoft.EntityFrameworkCore;

using ZDeals.Engine.Data.Entities;

namespace ZDeals.Engine.Data
{
    public class EngineDbContext: DbContext
    {
        public EngineDbContext() { }

        public EngineDbContext(DbContextOptions<EngineDbContext> options) : base(options) { }


        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<VisitedPageEntity> VisitedPages { get; set; }

        public DbSet<TrackedPageEntity> TrackedPages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PriceHistoryEntity>(e =>
            {
                e.HasKey(p => new { p.ProductId, p.Sequence });
                e.HasOne(p => p.Product).WithMany(pr => pr.PriceHistory).HasForeignKey(p => p.ProductId);
            });
        }
    }
}
