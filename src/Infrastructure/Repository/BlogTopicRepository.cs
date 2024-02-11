using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Infrastructure.Repository
{
    public class BlogTopicRepository : IBlogTopicRepository
    {
        private readonly AppDbContext _context;

        public BlogTopicRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BlogTopic?> AddAsync(CreateBlogTopicBody blogBody)
        {
            var blog = new BlogTopic
            {
                Name = blogBody.TopicName,
                ShortDescription = blogBody.ShortDescription,
                Description = blogBody.Description,
            };

            blog = (await _context.Blogs.AddAsync(blog))?.Entity;
            await _context.SaveChangesAsync();

            return blog;
        }

        public async Task<BlogTopic?> GetAsync(Guid id)
            => await _context.Blogs.FindAsync(id);

        public async Task<IEnumerable<BlogTopic>> GetAll(int count, int countSkipped, bool isDesc = true)
        {
            var query = _context.Blogs.AsQueryable();

            if (isDesc)
                query = query.OrderByDescending(e => e.Date);
            else
                query = query.OrderBy(e => e.Date);

            return await query
                .Skip(countSkipped)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return true;

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}