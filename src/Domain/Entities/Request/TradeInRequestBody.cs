using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class TradeInRequestBody
    {
        [Required]
        public string Message { get; set; }
    }
}