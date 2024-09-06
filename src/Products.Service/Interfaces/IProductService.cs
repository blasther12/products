using Products.Infrastructure.Interfaces;
using Products.Service.DTOs;

namespace Products.Service.Interfaces
{
    public interface IProductService
    {
        void Delete(int id);

        ProductReadDto GetById(int id);

        ProductReadDto GetByName(string name);

        List<ProductReadDto> ListRecords();

        void Register(ProductCreateDto dto);

        void Update(ProductUpdateDto dto);
    }
}