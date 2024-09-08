using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Infrastructure.DataSeeding
{
    public class ProductSeeder(ProductsDbContext context)
    {
        private readonly ProductsDbContext _context = context;

        public async Task SeedAsync()
        {
            if (_context.Product.Any())
            {
                return;
            }

            var products = new[]
            {
                new Domain.Entities.Product { Name = "Product 1", Description = "Description 1", Value = 10.00m, CreatedDate = DateTime.UtcNow },
                new Domain.Entities.Product { Name = "Product 2", Description = "Description 2", Value = 20.00m, CreatedDate = DateTime.UtcNow },
                new Domain.Entities.Product { Name = "Product 3", Description = "Description 3", Value = 30.00m, CreatedDate = DateTime.UtcNow },
                new Domain.Entities.Product { Name = "Product 4", Description = "Description 4", Value = 40.00m, CreatedDate = DateTime.UtcNow },
                new Domain.Entities.Product { Name = "Product 5", Description = "Description 5", Value = 50.00m, CreatedDate = DateTime.UtcNow }
            };

            _context.Product.AddRange(products);
            await _context.SaveChangesAsync();
        }
    }
}