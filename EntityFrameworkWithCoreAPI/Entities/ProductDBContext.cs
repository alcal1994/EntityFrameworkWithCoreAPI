using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkWithCoreAPI.Entities
{
    public class ProductDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> User { get; set; }

        public ProductDBContext(DbContextOptions<ProductDBContext> options)
    : base(options)
        { }
    }
}
