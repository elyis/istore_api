using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class UpdateCharacteristicColorBody
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression("^[0-9a-fA-F]{6}$|^[0-9a-fA-F]{3}$")]
        public string Hex { get; set; }

        [Required]
        public string Color { get; set; }
    }
}