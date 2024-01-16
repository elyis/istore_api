namespace istore_api.src.Domain.Entities.Request
{
    public class ImagesWithColor
    {
        public List<string> Filenames { get; set; } = new();
        public string Color { get; set; }
        public string Hex { get; set; }
    }

    public class ImageUrlWithColor
    {
        public string UrlImage { get; set; }
        public string Hex { get; set; } 
        public string Color { get; set; }
    }
}