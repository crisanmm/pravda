using NewsStorage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace NewsStorage.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("newsstorage");
            modelBuilder.Entity<CachedClassified>(b =>
            {
                b.HasKey(e => e.id);
                b.Property(e => e.id).UseIdentityColumn(1, 1);
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CachedClassified> Classifieds { get; set; }
    }
}