using System.Text;
using istore_api.src.Domain.Entities.Response;

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

        public override string ToString()
        {
            var orderMessageBody = ToOrderMessageBody();
            var result = new StringBuilder();
            result.AppendLine($"{orderMessageBody.Name} * {orderMessageBody.Count}:");

            foreach(var characteristic in orderMessageBody.Characteristics){
                var temp = $"   {characteristic.Name}: {characteristic.Value}";
                result.AppendLine(temp);
            }
            return result.ToString();
        }
    }
}