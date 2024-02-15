namespace istore_api.src.Domain.Entities.Response
{
    public class ProductAnalyticBody
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<string> Images { get; set; } = new();
    }
}