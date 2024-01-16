using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductCharacteristicController : ControllerBase
    {
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;
        private readonly IProductRepository _productRepository;

        public ProductCharacteristicController(
            IProductCharacteristicRepository productCharacteristicRepository,
            IProductRepository productRepository
        )
        {
            _productCharacteristicRepository = productCharacteristicRepository;
            _productRepository = productRepository;
        }


        [HttpPost("characteristic")]
        [SwaggerOperation("Создать характеристику товара")]
        [SwaggerResponse(200)]
        [SwaggerResponse(409)]

        public async Task<IActionResult> CreateCharacteristic(CreateProductCharacteristicBody characteristicBody)
        {
            var product = await _productRepository.GetAsync(characteristicBody.ProductId);
            if(product == null)
                return BadRequest("productId not found");

            var result = await _productCharacteristicRepository.AddAsync(characteristicBody, product);
            return result == null ? Conflict() : Ok();
        }

        [HttpPost("characteristics")]
        [SwaggerOperation("Получить список характеристик")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<string>))]

        public async Task<IActionResult> GetCharacteristics()
        {
            var productCharacteristics = await _productCharacteristicRepository.GetAllAsync();
            return Ok(productCharacteristics);
        }
    }
}