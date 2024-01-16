using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Entities.Request
{
    public class CreatePromoCodeBody
    {
        [EnumDataType(typeof(PromoCodeType))]
        public PromoCodeType Type { get; set; }
        public float Value { get; set; }

        [Range(1, short.MaxValue)]
        public int CountDays { get; set; }
    }
}