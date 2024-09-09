using Microsoft.EntityFrameworkCore;

namespace Products.Infrastructure.DataSeeding
{
    public class ProductSeeder(ProductsDbContext context)
    {
        private readonly ProductsDbContext _context = context;

        public async Task SeedAsync()
        {
            if (await _context.Product.AnyAsync())
            {
                return;
            }

            var products = new[]
            {
                new Domain.Entities.Product { Name = "Product 1", Description = "Description 1", Quantity = 10, Value = 10.00m, CreatedDate = DateTime.UtcNow },
                new Domain.Entities.Product { Name = "Product 2", Description = "Description 2", Quantity = 10, Value = 20.00m, CreatedDate = DateTime.UtcNow },
                new Domain.Entities.Product { Name = "Product 3", Description = "Description 3", Quantity = 10, Value = 30.00m, CreatedDate = DateTime.UtcNow },
                new Domain.Entities.Product { Name = "Product 4", Description = "Description 4", Quantity = 10, Value = 40.00m, CreatedDate = DateTime.UtcNow },
                new Domain.Entities.Product { Name = "Product 5", Description = "Description 5", Quantity = 10, Value = 50.00m, CreatedDate = DateTime.UtcNow }
            };

            await _context.Product.AddRangeAsync(products);
            
            await _context.SaveChangesAsync();
        }
    }
}