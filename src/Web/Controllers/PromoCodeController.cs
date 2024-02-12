using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeRepository _promoCodeRepository;

        public PromoCodeController(IPromoCodeRepository promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
        }

        [HttpPost("promocode"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Создать промокод")]
        [SwaggerResponse(200, Type = typeof(PromoCodeBody))]
        public async Task<IActionResult> CreatePromoCode(CreatePromoCodeBody promoCodeBody)
        {
            var result = await _promoCodeRepository.AddAsync(promoCodeBody);
            return result == null ? BadRequest() : Ok(result.ToPromoCodeBody());
        }

        [HttpGet("promocodes"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Получить список промокодов")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<PromoCodeBody>))]

        public async Task<IActionResult> GetAll()
        {
            var promocodes = await _promoCodeRepository.GetAllAsync(false);
            var result = promocodes.Select(e => e.ToPromoCodeBody());
            return Ok(result);
        }

        [HttpGet("promocode")]
        [SwaggerOperation("Проверить промокод")]
        [SwaggerResponse(200, Type = typeof(PromoCodeBody))]
        [SwaggerResponse(400)]

        public async Task<IActionResult> GetCode([FromQuery, Required] string code)
        {
            var promocode = await _promoCodeRepository.GetOrRemoveExpiredAsync(code);
            if (promocode == null)
                return BadRequest();

            return promocode.IsActive == false ? Ok(promocode.ToPromoCodeBody()) : BadRequest();
        }


        [HttpDelete("promocode"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Удалить промокод")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> RemoveCode([FromQuery, Required] string code)
        {
            var isRemoved = await _promoCodeRepository.RemoveAsync(code);
            return isRemoved ? NoContent() : BadRequest();
        }

    }
}