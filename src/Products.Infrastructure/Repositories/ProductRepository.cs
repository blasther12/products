using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infrastructure.Interfaces;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository(ProductsDbContext dbContext) : Repository<Product>(dbContext), IProductRepository
    {     
        private readonly ProductsDbContext _dbContext = dbContext;

        public Product GetByName(int id)
        {
            throw new NotImplementedException();
        }
    }
}