using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Domain.Models
{
    [Index(nameof(Code))]
    public class PromoCode
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public float Value { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime DateExpiration { get; set; }


        public PromoCodeBody ToPromoCodeBody()
        {
            return new PromoCodeBody
            {
                Code = Code,
                Type = Enum.Parse<PromoCodeType>(Type),
                Value = Value,
                DateExpiration = DateExpiration.ToString("yyyy-MM-ddTHH:mm:ss")
            };
        }
    }
}