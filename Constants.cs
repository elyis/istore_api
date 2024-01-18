namespace istore_api
{
    public static class Constants
    {
        public static readonly string serverUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";").First();
        // public static readonly string serverUrl = "https://d75f-95-183-28-39.ngrok-free.app";

        public static readonly string localPathToStorages = @"Resources/";
        public static readonly string localPathToProfileIcons = $"{localPathToStorages}ProfileIcons/";
        public static readonly string localPathToProductIcons = $"{localPathToStorages}ProductIcons/";

        public static readonly string webPathToProfileIcons = $"{serverUrl}/api/upload/profileIcon/";
        public static readonly string webPathToProductIcons = $"{serverUrl}/api/upload/productIcon/";
    }
}