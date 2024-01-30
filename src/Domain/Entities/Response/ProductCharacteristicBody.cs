using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Entities.Response
{
    public class ProductCharacteristicBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CharacteristicType Type { get; set; }
        public ProductCharacteristicElem Characteristic { get; set; }
    }

    public class ProductCharacteristicElem
    {
        public string? Color { get; set; }
        public string? Hex { get; set; }
        public List<string> Values { get; set; } = new();
    }

    public class FilterCharacteristic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CharacteristicType Type { get; set; }
        public List<ProductCharacteristicElem> Elems { get; set; } = new();
    }

}