namespace istore_api.src.Domain.Entities.Request
{
    public class CreateProductCharacteristicBody
    {
        public string Name { get; set; }
        public List<ChangeInCostBody> Variants { get; set; } = new();
        public Guid ProductId { get; set; }
    }

    public class ChangeInCostBody
    {
        public string Value { get; set; }
        public float IncreaseInPrice { get; set; } = 0.0f;
    }
}