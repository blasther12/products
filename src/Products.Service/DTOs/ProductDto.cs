namespace Products.Service.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
    }

    public class ProductCreateDto : ProductDto { }

    public class ProductUpdateDto : ProductDto
    {
        public long Id { get; set; }
    }

    public class ProductReadDto : ProductDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
    }

    public class ProductListDto
    {
        public List<ProductReadDto> Products { get; set; }
        public int TotalCount { get; set; }
    }
}