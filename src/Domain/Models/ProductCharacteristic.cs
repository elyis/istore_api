using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Models
{
    public class ProductCharacteristic
    {
        [Key]
        public string Name { get; set; }

        public List<ProductCharacteristicVariant> CharacteristicVariants { get; set; } = new();
    }
}