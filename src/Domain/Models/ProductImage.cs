using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Response;

namespace istore_api.src.Domain.Models
{
    public class ProductImage
    {
        [Key]
        public string Filename { get; set; }
        public bool IsPreviewImage { get; set; }
        public string Color { get; set; }
        public string Hex { get; set; }
        
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public List<OptionsCombiningCharacteristic> OptionsCombiningCharacteristic { get; set; } = new();

        public Elem ToElem()
        {
            return new Elem
            {
                Color = Color,
                Hex = Hex,
                Value = $"{Constants.webPathToProductIcons}{Filename}"
            };
        }
    }
}