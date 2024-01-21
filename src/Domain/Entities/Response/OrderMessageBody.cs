namespace istore_api.src.Domain.Entities.Response
{
    public class OrderMessageBody
    {
        public string Name { get; set; }
        public List<CharacteristicValuePair> Characteristics { get; set; } = new();
        public int Count { get; set; }
    }

    public class CharacteristicValuePair
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}