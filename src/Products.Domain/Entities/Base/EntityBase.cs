using System.ComponentModel.DataAnnotations;

namespace Products.Domain.Entities.Base
{
    public class EntityBase
    {
        [Key]
        public long Id { get; set; }
    }
}