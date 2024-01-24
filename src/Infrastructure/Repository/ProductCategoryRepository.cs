using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using istore_api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Infrastructure.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductCategory?> AddAsync(ProductCategoryBody categoryBody)
        {
            var category = await GetAsync(categoryBody.Name);
            if(category != null)
                return null;

            category = new ProductCategory
            {
                Name = categoryBody.Name
            };


            category = (await _context.ProductCategories.AddAsync(category))?.Entity;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
            => await _context.ProductCategories.ToListAsync();

        public async Task<ProductCategory?> GetAsync(Guid id)
            => await _context.ProductCategories.FindAsync(id);

        public async Task<ProductCategory?> GetAsync(string name)
        {
            var nameLowerCase = name.ToLower();
            return await _context.ProductCategories
                .FirstOrDefaultAsync(e => e.Name.ToLower() == nameLowerCase);
        }

        public async Task<bool> RemoveAsync(string name)
        {
            var productCategory = await GetAsync(name);
            if(productCategory == null)
                return true;

            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}