using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory?> GetAsync(Guid id);
        Task<ProductCategory?> GetAsync(string name);
        Task<ProductCategory?> AddAsync(ProductCategoryBody categoryBody);
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task<bool> RemoveAsync(string name);
    }
}