namespace istore_api.src.Domain.Models
{
    public class OptionsCombiningCharacteristic
    {
        public string ImageFilename { get; set; }
        public string CharacteristicName { get; set; }

        public ProductCharacteristic ProductCharacteristic { get; set; }
        public ProductImage ProductImage { get; set; }
    }
}