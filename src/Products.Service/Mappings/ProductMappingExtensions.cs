using Products.Service.DTOs;

namespace Products.Service.Mappings
{
    public static class ProductMappingExtensions
    {
        public static Domain.Entities.Product ToEntity(this ProductDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new Domain.Entities.Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Value = dto.Value,
                Quantity = dto.Quantity,
                CreatedDate = DateTime.UtcNow,
            };
        }

        public static Domain.Entities.Product ToEntityUpdate(this ProductUpdateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new Domain.Entities.Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Quantity = dto.Quantity,
                Value = dto.Value,
                CreatedDate = DateTime.UtcNow,
            };
        }

        public static ProductReadDto ToDTO(this Domain.Entities.Product entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new ProductReadDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Quantity = entity.Quantity,
                Value = entity.Value,
                Date = entity.CreatedDate,
            };
        }

        public static ProductListDto ToListDTO(this List<Domain.Entities.Product> entities)
        {
            return new ProductListDto
            {
                TotalCount = entities.Count,
                Products = entities.Select(x => x.ToDTO()).ToList(),
            };
        }
    }
}