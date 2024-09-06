using Products.Domain.Entities;
using Products.Infrastructure.Interfaces;
using Products.Service.DTOs;
using Products.Service.Interfaces;

namespace Products.Service
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ProductReadDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ProductReadDto GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<ProductReadDto> ListRecords()
        {
            throw new NotImplementedException();
        }

        public void Register(ProductCreateDto productCreateDto)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductUpdateDto productUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}