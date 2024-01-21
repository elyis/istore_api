using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Shared;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.Models;

namespace istore_api.src.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<User?> AddAsync(SignUpBody body, string role);
        Task<User?> GetAsync(Guid id);
        Task<User?> GetAsync(string email);
        Task<string?> UpdateTokenAsync(string refreshToken, Guid userId, TimeSpan? duration = null);
        Task<User?> GetByTokenAsync(string refreshTokenHash);
        Task<User?> UpdateProfileIconAsync(Guid userId, string filename);
        Task<IEnumerable<User>> GetAllByRole(UserRole role);
        Task<IEnumerable<User>> GetAllOrUpdateByChatId(IEnumerable<TelegramBotUserInfo> userInfos);
    }
}