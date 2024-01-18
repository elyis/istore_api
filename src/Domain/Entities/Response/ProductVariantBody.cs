using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Entities.Response
{
    public class ProductVariantBody
    {
        public string? Color { get; set; }
        public List<CharacteristicPairValue> Characteristics { get; set; } = new();
        public List<string> ImageUrls { get; set; } = new();
        public float TotalPrice { get; set; }
    }

    public class CharacteristicPairValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public float PriceModifier { get; set; }
    }

    public class Filter
    {
        public string Name { get; set; }
        public CharacteristicType Type { get; set; }
        public List<Elem> Elems { get; set; }
    }

    public class Elem
    {
        public string Value { get; set; }
        public string? Color { get; set; }
        public string? Hex { get; set; }
    }
}