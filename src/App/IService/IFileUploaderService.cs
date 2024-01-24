namespace webApiTemplate.src.App.IService
{
    public interface IFileUploaderService
    {
        Task<List<string>> UploadFileAsync(string directoryPath, Stream stream, List<string> fileExtensions);
        Task<byte[]?> GetStreamFileAsync(string directoryPath, string filename);
        Task<bool> RemoveFileAsync(string directoryPath, string filename);
    }
}