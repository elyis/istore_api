using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IBlogTopicRepository
    {
        Task<IEnumerable<BlogTopic>> GetAll(int count, int countSkipped, bool isDesc = true);
        Task<BlogTopic?> AddAsync(CreateBlogTopicBody blogBody);
    }
}