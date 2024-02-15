namespace istore_api.src.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string CommunicationMethod { get; set; }
        public string? Comment { get; set; }
        public float TotalSum { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderProduct> Products { get; set; } = new();
    }
}