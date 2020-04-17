using Microsoft.EntityFrameworkCore;
using ZDeals.Identity.Data.Entities;

namespace ZDeals.Identity.Data
{
    public class ZIdentityDbContext: DbContext
    {
        public ZIdentityDbContext() { }
        
        public ZIdentityDbContext(DbContextOptions<ZIdentityDbContext> options): base(options) { }
        

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
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
