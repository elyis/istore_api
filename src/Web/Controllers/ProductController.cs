using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IDeviceModelRepository _deviceModelRepository; 

        public ProductController(
            IProductRepository productRepository,
            IDeviceModelRepository deviceModelRepository
        )
        {
            _productRepository = productRepository;
            _deviceModelRepository = deviceModelRepository;
        }

        [HttpGet("products/{patternName}")]
        [SwaggerOperation("Получить все продукты")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<ProductBody>))]
        public async Task<IActionResult> GetProductsByPatternName(string patternName)
        {
            if(string.IsNullOrEmpty(patternName))
                return BadRequest("pattern is empty or null");

            var products = await _productRepository.GetAllByPatternName(patternName);
            return Ok(products);
        }


        [HttpGet("products")]
        [SwaggerOperation("Получить все продукты")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<ProductBody>))]
        public async Task<IActionResult> GetProductsByDeviceName([FromQuery] string deviceModel)
        {
            var products = new List<Product>();
            products = (await _productRepository.GetAll(deviceModel)).ToList();
            if(!products.Any())
            {
                var deviceModels = (await _deviceModelRepository.GetAllAsync(deviceModel)).Select(e => e.Name);
                
                var tasks = new List<Task<IEnumerable<Product>>>();
                foreach(var model in deviceModels)
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


        [HttpPost("product")]
        [SwaggerOperation("Создать товар")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> CreateProduct(CreateProductBody productBody)
        {
            var deviceModel = await _deviceModelRepository.GetAsync(productBody.ModelName);
            if(deviceModel == null)
                return BadRequest("device model name is not found");

            var result = await _productRepository.AddAsync(deviceModel, productBody);
            return result == null ? BadRequest() : Ok();
        }

        [HttpPut("product")]
        [SwaggerOperation("Изменить товар")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> UpdateProduct(UpdatingProductBody productBody)
        {
            var result = await _productRepository.UpdateAsync(productBody);
            return result == null ? Ok() : BadRequest("id not found");
        }
    }
}