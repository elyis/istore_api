using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Entities.Request
{
    public class CreateOrderBody
    {
        [Required]
        public string Fullname { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [EnumDataType(typeof(CommunicationMethod))]
        public CommunicationMethod CommunicationMethod { get; set; }
        
        public string? Comment { get; set; }
        public string? PromoCode { get; set; }

        public List<ConfigurationBody> Configurations { get; set; } = new();
    }

    public class ConfigurationBody
    {
        public Guid ConfigurationId { get; set; }
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }
}