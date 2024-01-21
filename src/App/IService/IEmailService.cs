namespace istore_api.src.App.IService
{
    public interface IEmailService
    {
        Task<bool> SendMessage(string email, string subject, string message);
    }
}