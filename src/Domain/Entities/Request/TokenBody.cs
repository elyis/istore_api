using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class TokenBody
    {
        [Required]
        public string Value { get; set; }
    }
}