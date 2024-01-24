using istore_api.src.Domain.Entities.Response;

namespace istore_api.src.Domain.Models
{
    public class BlogTopic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;


        public BlogTopicBody ToBlogTopicBody()
        {
            return new BlogTopicBody
            {
                Id = Id,
                Name = Name,
                Description = Description,
                ShortDescription = ShortDescription
            };
        }
    }
}