using Microsoft.EntityFrameworkCore;
using ZDeals.Data.Entities.Sales;

namespace ZDeals.Data
{
    public class ZDealsDbContext: DbContext
    {
        public ZDealsDbContext() { }
        
        public ZDealsDbContext(DbContextOptions<ZDealsDbContext> options): base(options) { }
        

        public DbSet<DealEntity> Deals { get; set; }
        public DbSet<StoreEntity> Stores { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<DealPictureEntity> DealPictures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StoreEntity>(e =>
            {
                e.HasIndex(s => s.Name).IsUnique(true);
            });                

            builder.Entity<DealEntity>(e =>
            {
                e.HasOne(d => d.Store).WithMany(s => s.Deals).HasForeignKey(d => d.StoreId);
            });                

            builder.Entity<CategoryEntity>(e =>
            {
                e.HasOne(c => c.Parent).WithMany(c => c.Children).HasForeignKey(c => c.ParentId);
                e.HasIndex(c => c.Code).IsUnique(true);
            });

            builder.Entity<DealCategoryJoin>(dc =>
            {
                dc.HasKey(e => new { e.DealId, e.CategoryId });
                dc.HasOne(e => e.Deal).WithMany(e => e.DealCategory).HasForeignKey(e => e.DealId).IsRequired(true);
                dc.HasOne(e => e.Category).WithMany(e => e.DealCategory).HasForeignKey(e => e.CategoryId).IsRequired(true);
            });

            builder.Entity<DealPictureEntity>(dp =>
            {
                dp.HasKey(e => new { e.FileName, e.DealId });
                dp.HasOne(e => e.Deal).WithMany(e => e.Pictures).HasForeignKey(e => e.DealId).IsRequired(true);
            });
        }
    }
}
