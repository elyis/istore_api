using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IProductCharacteristicRepository
    {
        Task<ProductCharacteristicVariant?> AddAsync(CreateProductCharacteristicBody characteristicBody, Product product);
        Task<IEnumerable<ProductCharacteristic>> GetAllAsync();
    }
}