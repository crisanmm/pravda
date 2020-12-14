using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserService.Entities;

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

            modelBuilder.Entity<User>()
                .ToTable("User");

        }
    }
}
