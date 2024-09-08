using Products.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Products.Domain.Entities
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Value cannot be less than 0")]
        public decimal Value { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}