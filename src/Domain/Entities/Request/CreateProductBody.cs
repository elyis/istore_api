namespace istore_api.src.Domain.Entities.Request
{
    public class CreateProductBody
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string ModelName { get; set; }
    }
}