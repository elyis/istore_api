namespace istore_api.src.Domain.Entities.Shared.Utility
{
    public static class CodeGenerator
    {
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random random = new();

        public static string Generate(int length)
        {
            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = Characters[random.Next(Characters.Length)];
            }

            return new string(code);
        }
    }
}