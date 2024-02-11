using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IBlogTopicRepository
    {
        Task<IEnumerable<BlogTopic>> GetAll(int count, int countSkipped, bool isDesc = true);
        Task<bool> RemoveAsync(Guid id);
        Task<BlogTopic?> GetAsync(Guid id);
        Task<BlogTopic?> AddAsync(CreateBlogTopicBody blogBody);
    }
}