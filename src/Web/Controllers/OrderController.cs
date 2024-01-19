using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPromoCodeRepository _promoCodeRepository;

        public OrderController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IPromoCodeRepository promoCodeRepository
        )
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _promoCodeRepository = promoCodeRepository;
        }

        [HttpPost("order")]
        [SwaggerOperation("Создать заказ")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> CreateOrder(CreateOrderBody orderBody)
        {
            if(!orderBody.Configurations.Any())
                return BadRequest("empty configuration");

            var configIds = orderBody.Configurations.Select(e => e.ConfigurationId).ToHashSet();
            var configs = (await _productRepository.GetAll(configIds)).ToList();

            if(configs.Count != configIds.Count())
                return BadRequest();

            float totalSum = orderBody.Configurations
                .Sum(config => 
                    configs.First(e => e.Id == config.ConfigurationId).Price * config.Count);


            if(orderBody.PromoCode != null)
            {
                var promocode = await _promoCodeRepository.GetOrRemoveExpiredAsync(orderBody.PromoCode);
                if(promocode == null)
                    return BadRequest("promocode is not found");

                else if(promocode != null && !promocode.IsActive)
                {
                    var promocodeType = Enum.Parse<PromoCodeType>(promocode.Type);

                    if(promocodeType == PromoCodeType.DiscountAmount)
                    {
                        totalSum -= promocode.Value;
                        totalSum = Math.Max(totalSum, 0);
                    }
                    else if(promocodeType == PromoCodeType.DiscountPercentage)
                        totalSum -= totalSum / promocode.Value;
                }

                else
                    return BadRequest("promocode is activated or expired");
            }

            var order = await _orderRepository.AddAsync(orderBody, configs, totalSum);
            if (order == null)
                return BadRequest();

            if(orderBody.PromoCode != null)
            {
                var result = await _promoCodeRepository.ActivePromocode(orderBody.PromoCode);
                if(result == false)
                    return BadRequest();
            }

            return order == null ? BadRequest() : Ok();
        }
    }
}