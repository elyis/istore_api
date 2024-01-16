namespace istore_api.src.Domain.Entities.Response
{
    public class ProductCharacteristicVariantBody
    {
        public string CharacteristicName { get; set; }
        public List<string> Values { get; set; } = new();
        public List<float> GrowthToValues { get; set; } = new();
    }
}