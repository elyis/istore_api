using SixLabors.ImageSharp;
using webApiTemplate.src.App.IService;

namespace webApiTemplate.src.App.Service
{
    public class LocalFileUploaderService : IFileUploaderService
    {
        public async Task<List<string>> UploadFileAsync(string directoryPath, Stream stream, List<string> fileExtensions)
        {
            var filenames = new List<string>();
            if (fileExtensions.Count == 0 || fileExtensions.Any(string.IsNullOrEmpty))
                return filenames;
            
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach(var fileExtension in fileExtensions)
            {
                string filename = $"{Guid.NewGuid()}.{fileExtension}";
                string fullPathToFile = Path.Combine(directoryPath, filename);

                if(fileExtension == "webp")
                    await ConvertImageToWebpFormat(stream, fullPathToFile);
                else
                {
                    using var file = File.Create(fullPathToFile);
                    stream.Seek(0, SeekOrigin.Begin);
                    await stream.CopyToAsync(file);
                }
                
                filenames.Add(filename);
            }
            return filenames;
        }

        public async Task<bool> RemoveFileAsync(string directoryPath, string filename)
        {
            if(string.IsNullOrEmpty(filename) && !Directory.Exists(directoryPath))
                return false;

            var fullpath = Path.Combine(directoryPath, filename);
            if(!File.Exists(fullpath))
                return false;

            await Task.Run(() => File.Delete(fullpath));
            return true;
        }

        public async Task<byte[]?> GetStreamFileAsync(string directoryPath, string filename)
        {
            string fullPathToFile = Path.Combine(directoryPath, filename);
            if (!File.Exists(fullPathToFile))
                return null;

            using Stream fileStream = File.OpenRead(fullPathToFile);
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        private static async Task ConvertImageToWebpFormat(Stream stream, string path)
        {
            stream.Position = 0;
            using var image = await Image.LoadAsync(stream);
            using var memoryStream = new MemoryStream();
            await image.SaveAsWebpAsync(path);
        }
    }
}