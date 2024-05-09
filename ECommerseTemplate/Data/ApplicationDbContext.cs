using ECommerseTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerseTemplate.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
 
        }

        // Db tables
        public DbSet<CategoryModel> Categories { get; set; }
    }
}
