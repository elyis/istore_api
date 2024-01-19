namespace istore_api.src.Domain.Models
{
    public class OrderProduct
    {
        public Guid OrderId { get; set; }
        public Guid ProductConfigurationId { get; set; } 

        public Order Order { get; set; }
        public ProductConfiguration ProductConfiguration { get; set; }
        public int Count { get; set; }
    }
}