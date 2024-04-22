using istore_api.src.Domain.Entities.Response;

namespace istore_api.src.Domain.Models
{
    public class ProductConfiguration
    {
        public Guid Id { get; set; }
        public float Price { get; set; }

        public Product Product { get; set; }
        public Guid ProductId { get; set; }

        public List<ProductConfigCharacteristic> Characteristics { get; set; } = new();

        public ProductConfigurationBody ToProductConfigCharacteristic()
            => new()
            {
                ConfigurationId = Id,
                TotalPrice = Price,
                Characteristics = Characteristics.Select(e => e.ToCharacteristicPair()).ToList(),
            };
    }
}