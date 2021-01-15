using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserService.Entities;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace UserService.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("userservice");
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).UseIdentityColumn(1, 1);
            });
            modelBuilder.Entity<User>()
                .ToTable("User");
        }
    }
}
