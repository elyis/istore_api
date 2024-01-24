using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IProductRepository
    {
        Task<Product?> GetAsync(Guid id);
        Task<IEnumerable<Product>> GetAllByPatternName(string pattern);
        Task<IEnumerable<Product>> GetAll(string deviceModelName);
        Task<Product?> AddAsync(DeviceModel deviceModel, CreateProductBody productBody);
        Task<Product?> UpdateAsync(UpdatingProductBody productBody);
        Task<IEnumerable<ProductConfiguration>> GetAll(IEnumerable<Guid> configIds);
        Task<bool> RemoveAsync(Guid id);
    }
}