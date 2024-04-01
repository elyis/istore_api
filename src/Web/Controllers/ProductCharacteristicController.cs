using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductCharacteristicController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCharacteristicRepository _productCharacteristicRepository;

        public ProductCharacteristicController(
            IProductRepository productRepository,
            IProductCharacteristicRepository productCharacteristicRepository
        )
        {
            _productRepository = productRepository;
            _productCharacteristicRepository = productCharacteristicRepository;
        }

        [HttpPut("characteristic"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Изменить цвет товара")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> UpdateCharacteristicColor(UpdateCharacteristicColorBody colorBody)
        {
            var result = await _productCharacteristicRepository.UpdateCharacteristicColor(colorBody);
            return result == null ? BadRequest() : Ok();
        }

        [HttpPost("characteristic"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Создать характеристику товара")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        [SwaggerResponse(409)]

        public async Task<IActionResult> CreateCharacteristic(
            CreateCharacteristicBody characteristicBody,
            [FromQuery, Required] Guid productId
        )
        {
            if (productId == Guid.Empty)
                return BadRequest("productId is empty");

            var product = await _productRepository.GetAsync(productId);
            if (product == null)
                return NotFound("productId not found");

            var result = await _productCharacteristicRepository.AddAsync(product, characteristicBody, CharacteristicType.Text);
            return result == null ? Conflict() : Ok();
        }


        [HttpGet("characteristics")]
        [SwaggerOperation("Получить список характеристик")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<ProductCharacteristicBody>))]

        public async Task<IActionResult> GetCharacteristics([Required] Guid productId)
        {
            var characteristics = await _productCharacteristicRepository.GetAll(productId);
            var result = characteristics.Select(e => e.ToProductCharacteristicBody());
            return Ok(result);
        }

        [HttpDelete("characteristic"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Удалить характеристику")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> RemoveProductCharacteristic([FromQuery, Required] Guid characteristicId)
        {
            var result = await _productCharacteristicRepository.RemoveAsync(characteristicId);
            return result ? NoContent() : BadRequest();
        }
    }
}