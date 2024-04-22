using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IInitialRegistrationRepository
    {
        Task<InitialRegistration?> Get(string email);
        Task<InitialRegistration?> Create(string email);
    }
}