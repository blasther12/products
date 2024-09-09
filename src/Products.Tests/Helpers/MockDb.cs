using Microsoft.EntityFrameworkCore;
using Products.Infrastructure;

namespace Products.Tests.Helpers
{
    public class MockDb : IDbContextFactory<ProductsDbContext>
    {
        public ProductsDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            return new ProductsDbContext(options);
        }
    }
}