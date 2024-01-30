using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string DeviceModelName { get; set; }
        public DeviceModel DeviceModel { get; set; }
        public List<ProductCharacteristic> ProductCharacteristics { get; set; } = new();
        public List<ProductConfiguration> ProductConfigurations { get; set; } = new();

        public List<OrderProduct> Orders { get; set; } = new();

        public List<FilterCharacteristic> ToFilterCharacteristics()
        {
            var filters = new List<FilterCharacteristic>();
            var colorType = CharacteristicType.Color.ToString();
            var textType = CharacteristicType.Text.ToString();

            var colorElems = ProductCharacteristics.Where(e => e.Type == colorType);
            if (colorElems.Any())
            {
                var colorFilter = new FilterCharacteristic
                {
                    Id = colorElems.First().Id,
                    Type = CharacteristicType.Color,
                    Name = CharacteristicType.Color.ToString(),
                    Elems = colorElems.Select(e => e.ToProductCharacteristicElem()).ToList()
                };

                filters.Add(colorFilter);
            }

            var textCharacteristics = ProductCharacteristics.Where(e => e.Type == textType);
            var textFilters = textCharacteristics.Select(e =>
                new FilterCharacteristic
                {
                    Id = e.Id,
                    Name = e.Name,
                    Type = CharacteristicType.Text,
                    Elems = e.Values.Split(";").Select(e =>
                    new ProductCharacteristicElem
                    {
                        Color = null,
                        Hex = null,
                        Values = new List<string> { e }
                    })
                    .ToList()
                });
            filters.AddRange(textFilters);
            return filters;
        }

        public ProductBody ToProductBody()
            => new()
            {
                ProductId = Id,
                Description = Description,
                Name = Name,
                Filters = ToFilterCharacteristics(),
                ProductConfigurations = ProductConfigurations.Select(e => e.ToProductConfigCharacteristic()).ToList()
            };
    }
}