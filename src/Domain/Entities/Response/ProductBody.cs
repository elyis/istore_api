namespace istore_api.src.Domain.Entities.Response
{
    public class ProductBody
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public List<Filter> Filters { get; set; } = new();
        public List<ProductVariantBody> ProductVariantBodies { get; set; } = new();
    }
}