using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryController
        (
            IProductCategoryRepository productCategoryRepository
        )
        {
            _productCategoryRepository = productCategoryRepository;
        }

        [HttpGet("productCategories")]
        [SwaggerOperation("Получить все категории")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<ProductCategoryBody>))]
        public async Task<IActionResult> GetProductCategories()
        {
            var categories = await _productCategoryRepository.GetAllAsync();
            var result = categories.Select(e => e.ToProductCategoryBody());
            return Ok(result);
        }

        [HttpPost("productCategory"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Создать категорию")]
        [SwaggerResponse(200)]
        [SwaggerResponse(409)]

        public async Task<IActionResult> CreateProductCategory(ProductCategoryBody categoryBody)
        {
            var result = await _productCategoryRepository.AddAsync(categoryBody);
            return result == null ? Conflict() : Ok(result.ToProductCategoryBody());
        }


        [HttpDelete("productCategory"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Удалить категорию продукта")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> RemoveDeviceModel([FromQuery, Required] string categoryName)
        {
            var result = await _productCategoryRepository.RemoveAsync(categoryName);
            return result ? NoContent() : BadRequest();
        }
    }
}