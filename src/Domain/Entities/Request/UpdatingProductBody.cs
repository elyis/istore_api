using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class UpdatingProductBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, float.MaxValue)]
        public float Price { get; set; }
    }
}