#nullable enable
using Products.Infrastructure.ExceptionHandling;
using Products.Infrastructure.Interfaces;
using Products.Service.DTOs;
using Products.Service.Interfaces;
using Products.Service.Mappings;

namespace Products.Service
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task Delete(long id)
        {
            await _productRepository.Delete(id);
        }

        public async Task<ProductReadDto?> GetById(long id)
        {
            var product = await _productRepository.GetById(id);

            return product?.ToDTO();
        }

        public async Task<ProductListDto> ListRecords(string? name, string? sortBy)
        {
            var products = await _productRepository.ListRecords(name, sortBy);

            return products.ToListDTO();
        }

        public async Task Register(ProductDto dto)
        {
            ValidatePayload(dto);
            await _productRepository.Register(dto.ToEntity());
        }

        public async Task Update(ProductUpdateDto dto)
        {
            ValidatePayload(dto);
            await _productRepository.Update(dto.ToEntityUpdate());
        }

        private static void ValidatePayload(ProductDto product)
        {
            if(string.IsNullOrEmpty(product.Name)) 
            {
                throw new ValidationException("Name cannot be null!");
            }

            if(product.Value < 0) 
            {
                throw new ValidationException("The product value cannot be less than zero!");
            }
        }
    }
}