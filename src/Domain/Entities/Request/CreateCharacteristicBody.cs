namespace istore_api.src.Domain.Entities.Request
{
    public class CreateCharacteristicBody
    {
        public string Name { get; set; }
        public List<string> Values { get; set; } = new();
    }
}