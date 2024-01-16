namespace istore_api.src.App.IService
{
    public interface IEmailService
    {
        Task SendMessage(string email, string subject, string message);
    }
}