using istore_api.src.Domain.Entities.Shared;

namespace istore_api.src.App.IService
{
    public interface ITelegramBotService
    {
        Task<List<TelegramBotUserInfo>> GetChatIdsAsync();
        Task SendMessageAsync(string message, IEnumerable<long> chatIds);
    }
}