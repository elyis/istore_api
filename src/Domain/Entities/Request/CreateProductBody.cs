using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class CreateProductBody
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public string ModelName { get; set; }
    }
}