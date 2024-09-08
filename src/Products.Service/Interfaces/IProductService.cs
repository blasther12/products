#nullable enable
using Products.Service.DTOs;

namespace Products.Service.Interfaces
{
    public interface IProductService
    {
        Task Delete(long id);

        Task<ProductReadDto?> GetById(long id);

        Task<ProductListDto> ListRecords(string? name, string? sortBy);

        Task Register(ProductDto dto);

        Task Update(ProductUpdateDto dto);
    }
}