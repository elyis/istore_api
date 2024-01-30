using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Entities.Response
{
    public class ProductBody
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<FilterCharacteristic> Filters { get; set; } = new();
        public List<ProductConfigurationBody> ProductConfigurations { get; set; } = new();
    }

    public class ProductConfigurationBody
    {
        public Guid ConfigurationId { get; set; }
        public List<CharacteristicPair> Characteristics { get; set; } = new();
        public float TotalPrice { get; set; }
    }

    public class CharacteristicPair
    {
        public CharacteristicType Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}