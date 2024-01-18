using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IProductCharacteristicRepository
    {
        Task<ProductCharacteristic?> AddAsync(Product product, CreateCharacteristicBody characteristicBody, CharacteristicType characteristicType);
        Task<ProductCharacteristic?> GetAsync(string name, Guid productId);
        Task<IEnumerable<ProductCharacteristic>> GetAll(Guid productId);
        Task AddImagesToProduct(List<ProductCharacteristic> productCharacteristics, Guid productId);
    }
}