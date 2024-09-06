using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Products.Domain.Entities;

namespace Products.Infrastructure
{
    public class ProductsDbContext(IConfiguration configuration) : DbContext
    {
        private readonly IConfiguration _configuration = configuration;

        public DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = 
                Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ?? 
                _configuration.GetConnectionString("DefaultConnection")!.Replace("{PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));


            Console.WriteLine(connectionString);
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}