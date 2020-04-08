using Microsoft.EntityFrameworkCore;
using ZDeals.Data.Entities.Identity;
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
        public DbSet<DealCategoryJoin> DealCategories { get; set; }
        public DbSet<DealPictureEntity> DealPictures { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

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

                //e.HasData(new CategoryEntity
                //{
                //    Id = 1,
                //    Code = "root",
                //    Title = "Categories",
                //    CreatedTime = System.DateTime.Now
                //});
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

            builder.Entity<UserEntity>(u =>
            {
                u.HasIndex(x => x.Username).IsUnique(true);
            });

            builder.Entity<RefreshTokenEntity>(t =>
            {
                t.HasIndex(x => x.Token).IsUnique(true);
                t.HasOne(x => x.User).WithMany(u => u.RefreshTokens).HasForeignKey(x => x.UserId).IsRequired(true);
            });
        }
    }
}
