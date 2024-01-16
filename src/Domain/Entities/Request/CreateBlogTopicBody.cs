namespace istore_api.src.Domain.Entities.Request
{
    public class CreateBlogTopicBody
    {
        public string TopicName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
    }
}