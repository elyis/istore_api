using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IPromoCodeRepository
    {
        Task<PromoCode?> AddAsync(CreatePromoCodeBody promoCodeBody);
        Task<PromoCode?> GetOrRemoveExpiredAsync(string code);
        Task<IEnumerable<PromoCode>> GetAllAsync(bool isActive);
        Task<bool> ActivePromocode(string code);
        Task<bool> RemoveAsync(string code);
    }
}