using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class RequestForDiscountBody
    {
        [Phone, Required]
        public string Phone { get; set; }

        [RegularExpression(@"^[А-ЯЁA-Za-z][а-яёA-Za-z]+\s+[А-ЯЁA-Za-z][а-яёA-Za-z]+\s*[А-ЯЁA-Za-z]?[а-яёA-Za-z]*$", ErrorMessage = "Invalid Fullname"), Required]
        public string Fullname { get; set; }
    }
}