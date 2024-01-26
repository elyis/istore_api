using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class CreateDeviceModelBody
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string ProductCategoryName { get; set; }
    }
}