using ECommerseTemplate.Models;

using Microsoft.EntityFrameworkCore;

namespace ECommerseTemplate.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
 
        }

        // Db tables
        public DbSet<CategoryModel> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel { Id = 1, Name = "Action", DisplayOrder = 1 },
                new CategoryModel { Id = 2, Name = "Sci-Fi", DisplayOrder = 2 },
                new CategoryModel { Id = 3, Name = "History", DisplayOrder = 3 }
                );
        }
    }
}
