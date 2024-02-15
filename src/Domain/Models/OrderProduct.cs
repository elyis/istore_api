using System.Text;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Models
{
    public class OrderProduct
    {
        public Guid OrderId { get; set; }
        public Guid ProductConfigurationId { get; set; }

        public Order Order { get; set; }
        public ProductConfiguration ProductConfiguration { get; set; }
        public int Count { get; set; }

        public OrderMessageBody ToOrderMessageBody()
        {
            return new OrderMessageBody
            {
                Name = ProductConfiguration.Product.Name,
                Characteristics = ProductConfiguration.Characteristics
                    .Select(e => new CharacteristicValuePair
                    {
                        Name = e.ProductCharacteristic.Name,
                        Value = e.Value
                    }).ToList(),
                Count = Count
            };
        }

        public ProductAnalyticBody ToProductAnalyticBody()
        {
            var characteristic = ProductConfiguration.Characteristics.FirstOrDefault(e => e.ProductCharacteristic.Type == CharacteristicType.Color.ToString());
            var productCharacteristic = characteristic?.ProductCharacteristic.ToProductCharacteristicElem();
            var filenames = productCharacteristic?.Values ?? new List<string>();


            return new ProductAnalyticBody
            {
                Count = Count,
                Name = ProductConfiguration.Product.Name,
                Images = filenames
            };
        }

        public override string ToString()
        {
            var orderMessageBody = ToOrderMessageBody();
            var result = new StringBuilder();
            result.AppendLine($"{orderMessageBody.Name} * {orderMessageBody.Count}:");

            foreach (var characteristic in orderMessageBody.Characteristics)
            {
                var temp = $"   {characteristic.Name}: {characteristic.Value}";
                result.AppendLine(temp);
            }
            return result.ToString();
        }
    }
}