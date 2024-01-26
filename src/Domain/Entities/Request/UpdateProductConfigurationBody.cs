using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class UpdateProductConfigurationBody
    {
        [Required]
        public Guid ConfigurationId { get; set; }

        [Range(0, float.MaxValue)]
        public float Price { get; set; }
    }
}