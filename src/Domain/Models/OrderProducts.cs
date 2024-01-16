namespace istore_api.src.Domain.Models
{
    public class OrderProducts
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; } 

        public Order Order { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}