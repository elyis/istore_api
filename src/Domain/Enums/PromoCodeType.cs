using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace istore_api.src.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PromoCodeType
    {
        DiscountAmount,
        DiscountPercentage
    }
}