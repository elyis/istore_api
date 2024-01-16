using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Entities.Response
{
    public class PromoCodeBody
    {
        public PromoCodeType Type { get; set; }
        public string Code { get; set; }
        public float Value { get; set; }
        public string DateExpiration { get; set; }
    }
}