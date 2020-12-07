using ClassificationService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassificationService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Classified> Classifieds { get; set; }
    }
}