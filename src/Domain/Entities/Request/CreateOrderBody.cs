using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Enums;

namespace istore_api.src.Domain.Entities.Request
{
    public class CreateOrderBody
    {
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        
        [EnumDataType(typeof(CommunicationMethod))]
        public CommunicationMethod CommunicationMethod { get; set; }
        public string? Comment { get; set; }
        public string? PromoCode { get; set; }

        public List<Purchase> Purchases { get; set; } = new();
    }

    public class Purchase
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public string? Color { get; set; }
        public List<CharacteristicPair> CharacteristicPairs { get; set; } = new();
    }

    public class CharacteristicPair
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}