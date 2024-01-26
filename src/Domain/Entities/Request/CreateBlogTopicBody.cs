using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class CreateBlogTopicBody
    {
        [Required]
        public string TopicName { get; set; }

        [Required]
        public string ShortDescription { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}