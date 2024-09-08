using Products.Domain.Entities;
using Products.Infrastructure.Interfaces.Base;

namespace Products.Infrastructure.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> ListRecords(string? name, string? sortBy);
    }
}