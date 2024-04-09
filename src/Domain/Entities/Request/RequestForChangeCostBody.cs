using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class RequestForChangeCostBody
    {
        [RegularExpression(@"^[А-ЯЁA-Za-z][а-яёA-Za-z]+\s+[А-ЯЁA-Za-z][а-яёA-Za-z]+\s*[А-ЯЁA-Za-z]?[а-яёA-Za-z]*$", ErrorMessage = "Invalid Fullname"), Required]
        public string Fullname { get; set; }

        [Phone, Required]
        public string Phone { get; set; }

        [Required]
        public string DeviceModel { get; set; }

        [Url(ErrorMessage = "Invalid Url"), Required]
        public string UrlToOtherStore { get; set; }
    }
}