using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Models
{
    public class ProductConfigCharacteristic
    {
        public ProductConfiguration ProductConfiguration { get; set; }
        public Guid ProductConfigurationId { get; set; }

        public ProductCharacteristic ProductCharacteristic { get; set; }
        public Guid ProductCharacteristicId { get; set; }
        public string Value { get; set; }


        public CharacteristicPair ToCharacteristicPair()
        => new()
        {
            Name = ProductCharacteristic.Name,
            Type = Enum.Parse<CharacteristicType>(ProductCharacteristic.Type),
            Value = Value
        };
    }
}