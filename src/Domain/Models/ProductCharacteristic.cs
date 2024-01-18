using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Models
{
    public class ProductCharacteristic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Values { get; set; }
        public string? Hex { get; set; }
        public string? Color { get; set; }


        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public List<ProductConfigCharacteristic> ProductConfigCharacteristics { get; set; } = new();


        public ProductCharacteristicBody ToProductCharacteristicBody()
            => new()
            {
                Id = Id,
                Name = Name,
                Type = Enum.Parse<CharacteristicType>(Type),
                Characteristic = ToProductCharacteristicElem()
            };


        public ProductCharacteristicElem ToProductCharacteristicElem()
            => new()
            {
                Color = Color,
                Hex = Hex,
                Values = Values.Split(";").ToList(),
            };
    }
}