using NewsStorage.Entities;
using Microsoft.EntityFrameworkCore;

namespace NewsStorage.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CachedClassified> Classifieds { get; set; }
    }
}