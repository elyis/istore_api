using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class UpdatingProductBody
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}