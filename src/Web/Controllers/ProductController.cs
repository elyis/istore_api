using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;
        private readonly IDeviceModelRepository _deviceModelRepository;

        public ProductController(
            IProductRepository productRepository,
            IProductCharacteristicRepository productCharacteristicRepository,
            IDeviceModelRepository deviceModelRepository
        )
        {
            _productRepository = productRepository;
            _productCharacteristicRepository = productCharacteristicRepository;
            _deviceModelRepository = deviceModelRepository;
        }


        [HttpGet("products")]
        [SwaggerOperation("Получить все продукты")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<ProductBody>))]
        public async Task<IActionResult> GetProductsByDeviceName([FromQuery, Required] string deviceModel)
        {
            var products = new List<Product>();
            products = (await _productRepository.GetAll(deviceModel)).ToList();
            if (!products.Any())
            {
                var deviceModels = (await _deviceModelRepository.GetAllAsync(deviceModel)).Select(e => e.Name);

                var tasks = new List<Task<IEnumerable<Product>>>();
                foreach (var model in deviceModels)
                {
                    var task = _productRepository.GetAll(model);
                    tasks.Add(task);
                }

                var results = await Task.WhenAll(tasks);
                products = results.SelectMany(e => e).ToList();
            }

            var result = products.Select(e => e.ToProductBody());
            return Ok(result);
        }

        [HttpPatch("product-configuration"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Обновить цену конфигурации")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, Description = "Конфигурация не найдена")]

        public async Task<IActionResult> UpdateProductConfiguration(UpdateProductConfigurationBody updateProductBody)
        {
            var result = await _productCharacteristicRepository.UpdateProductConfiguration(updateProductBody);
            return result == null ? NotFound() : Ok();
        }


        [HttpPost("product"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Создать товар")]
        [SwaggerResponse(200, Type = typeof(ProductBody))]
        [SwaggerResponse(400)]

        public async Task<IActionResult> CreateProduct(CreateProductBody productBody)
        {
            var deviceModel = await _deviceModelRepository.GetAsync(productBody.ModelName);
            if (deviceModel == null)
                return BadRequest("device model name is not found");

            var result = await _productRepository.AddAsync(deviceModel, productBody);
            return result == null ? BadRequest() : Ok(result.ToProductBody());
        }

        [HttpPut("product"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Изменить товар")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> UpdateProduct(UpdatingProductBody productBody)
        {
            var result = await _productRepository.UpdateAsync(productBody);
            return result == null ? BadRequest("id not found") : Ok();
        }

        [HttpDelete("product"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Удалить продукт")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> RemoveProductCharacteristic([FromQuery, Required] Guid productId)
        {
            var result = await _productRepository.RemoveAsync(productId);
            return result ? NoContent() : BadRequest();
        }

    }
}