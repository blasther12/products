using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infrastructure.Interfaces;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository(ProductsDbContext dbContext) : Repository<Product>(dbContext), IProductRepository
    {     
        private readonly ProductsDbContext _dbContext = dbContext;

        public async Task<List<Product>> ListRecords(string? name, string? sortBy)
        {
            var query = _dbContext.Product.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            query = sortBy?.ToLower() switch
            {
                "value" => query.OrderBy(p => p.Value),
                "date" => query.OrderBy(p => p.CreatedDate),
                _ => query.OrderBy(p => p.Name)
            };

            return await query.ToListAsync();
        }
    }
}