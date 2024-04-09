using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IInitialRegistrationRepository
    {
        Task<InitialRegistration?> GetByPhone(string phone);
        Task<InitialRegistration?> Create(string phone);
    }
}