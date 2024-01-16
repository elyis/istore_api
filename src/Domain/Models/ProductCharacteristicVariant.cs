using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Models
{
    public class ProductCharacteristicVariant
    {
        public ProductCharacteristic Characteristic { get; set; }
        public string CharacteristicName { get; set; }

        public Product Product { get; set; }
        public Guid ProductId { get; set; }

        public string Values { get; set; }
        public string GrowthToValues { get; set; }

        public ProductCharacteristicVariantBody ToProductCharacteristicVariantBody()
        {
            var values = Values.Split(";");
            var growthToValues = GrowthToValues.Split(";").Select(float.Parse); 
            return new ProductCharacteristicVariantBody
            {
                CharacteristicName = CharacteristicName,
                Values = values.ToList(),
                GrowthToValues = growthToValues.ToList(),
            };
        }

        public Filter ToFilter()
        {
            var filterByCharacteristic = new Filter
            {
                Name = CharacteristicName,
                Type = FilterType.Text,
                Elems = Values.Split(";").Select(e => new Elem
                {
                    Value = e
                }).ToList()
            };

            return filterByCharacteristic;
        }

        public List<CharacteristicPairValue> ToCharacteristicPairs()
        {
            var result = new List<CharacteristicPairValue>();
            var values = Values.Split(";");
            var growthToValues = GrowthToValues.Split(";");

            for(int i = 0; i < values.Length; i++)
            {
                var elem = new CharacteristicPairValue
                {
                    Name = CharacteristicName,
                    Value = values[i],
                    PriceModifier = float.Parse(growthToValues[i])
                };
                result.Add(elem);
            }
            return result;
        }
    }
}