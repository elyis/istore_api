using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IDeviceModelRepository
    {
        Task<DeviceModel?> GetAsync(string name);
        Task<DeviceModel?> AddAsync(CreateDeviceModelBody deviceModel, ProductCategory productCategory);
        Task<IEnumerable<DeviceModel>> GetAllAsync(string? productCategoryName);
        Task<bool> RemoveAsync(string name);
    }
}