namespace istore_api.src.Domain.Entities.Response
{
    public class BlogTopicBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
    }
}