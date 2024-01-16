using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class DeviceModelController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IDeviceModelRepository _deviceModelRepository;

        public DeviceModelController(
            IProductCategoryRepository productCategoryRepository,
            IDeviceModelRepository deviceModelRepository
        )
        {
            _productCategoryRepository = productCategoryRepository;
            _deviceModelRepository = deviceModelRepository;
        }

        [HttpGet("deviceModels")]
        [SwaggerOperation("Получить все модели в категории")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<string>))]
        [SwaggerResponse(404)]

        public async Task<IActionResult> GetDeviceModels([FromQuery] string categoryName)
        {
            var deviceModel = await _deviceModelRepository.GetAllAsync(categoryName);
            return deviceModel == null ? NotFound() : Ok(deviceModel.Select(e => e.Name)); 
        }


        [HttpPost("deviceModel")]
        [SwaggerOperation("Создать модель")]
        [SwaggerResponse(200)]
        [SwaggerResponse(409)]

        public async Task<IActionResult> CreateProductCategory(CreateDeviceModelBody deviceModel)
        {
            var productCategory = await _productCategoryRepository.GetAsync(deviceModel.ProductCategoryName);
            if(productCategory == null)
                return BadRequest();

            var result = await _deviceModelRepository.AddAsync(deviceModel, productCategory);
            return result == null ? Conflict() : Ok();
        }
    }
}