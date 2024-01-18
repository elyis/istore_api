using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.IRepository;
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

        [HttpPost("promocode")]
        [SwaggerOperation("Создать промокод")]
        [SwaggerResponse(200, Type = typeof(PromoCodeBody))]
        public async Task<IActionResult> CreatePromoCode(CreatePromoCodeBody promoCodeBody)
        {
            var result = await _promoCodeRepository.AddAsync(promoCodeBody);
            return result == null ? BadRequest() : Ok(result.ToPromoCodeBody());
        }

        [HttpGet("promocodes")]
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
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> GetCode([FromQuery] string code)
        {
            if(string.IsNullOrEmpty(code))
                return BadRequest();

            var promocode = await _promoCodeRepository.GetOrRemoveExpiredAsync(code);
            if(promocode == null)
                return BadRequest();

            return promocode.IsActive == false ? Ok() : BadRequest();
        }

    }
}