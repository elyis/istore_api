using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class ProductCategoryBody
    {
        [Required]
        public string Name { get; set; }
    }
}