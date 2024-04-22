using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Response;

namespace istore_api.src.Domain.Models
{
    public class DeviceModel
    {
        [Key]
        public string Name { get; set; }

        public string ProductCategoryName { get; set; }
        public ProductCategory ProductCategory { get; set; }

        public List<Product> Products { get; set; } = new();


        public DeviceModelBody ToDeviceModelBody()
        {
            return new DeviceModelBody
            {
                Name = Name
            };
        }
    }
}