using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IOrderRepository
    {
        Task<Order?> AddAsync(CreateOrderBody orderBody, List<ProductConfiguration> configurations, float totalSum);
        Task<Order?> GetAsync(Guid id);
    }
}