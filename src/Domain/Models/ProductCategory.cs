using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Response;

namespace istore_api.src.Domain.Models
{
    public class ProductCategory
    {
        [Key]
        public string Name { get; set; }

        public List<DeviceModel> DeviceModels { get; set; } = new();


        public ProductCategoryBody ToProductCategoryBody()
        {
            return new ProductCategoryBody
            {
                Name = Name
            };
        }
    }
}