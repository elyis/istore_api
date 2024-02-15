using System.Text;
using istore_api.src.App.IService;
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ITelegramBotService _telegramBotService;

        public OrderController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IPromoCodeRepository promoCodeRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            ITelegramBotService telegramBotService
        )
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _promoCodeRepository = promoCodeRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _telegramBotService = telegramBotService;
        }

        [HttpPost("order")]
        [SwaggerOperation("Создать заказ")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> CreateOrder(CreateOrderBody orderBody)
        {
            if (!orderBody.Configurations.Any())
                return BadRequest("empty configuration");

            var configIds = orderBody.Configurations.Select(e => e.ConfigurationId).ToHashSet();
            var configs = (await _productRepository.GetAll(configIds)).ToList();

            if (configs.Count != configIds.Count)
                return BadRequest();

            if (configs.Any(e => e.Price == 0))
                return BadRequest("The configuration price is not specified");

            float totalSum = orderBody.Configurations
                .Sum(config =>
                    configs.First(e => e.Id == config.ConfigurationId).Price * config.Count);


            if (orderBody.PromoCode != null)
            {
                var promocode = await _promoCodeRepository.GetOrRemoveExpiredAsync(orderBody.PromoCode);
                if (promocode == null)
                    return BadRequest("promocode is not found");

                else if (promocode != null && !promocode.IsActive)
                {
                    var promocodeType = Enum.Parse<PromoCodeType>(promocode.Type);

                    if (promocodeType == PromoCodeType.DiscountAmount)
                    {
                        totalSum -= promocode.Value;
                        totalSum = Math.Max(totalSum, 0);
                    }
                    else if (promocodeType == PromoCodeType.DiscountPercentage)
                        totalSum -= totalSum / promocode.Value;
                }

                else
                    return BadRequest("promocode is activated or expired");
            }

            var order = await _orderRepository.AddAsync(orderBody, configs, totalSum);
            if (order == null)
                return BadRequest();


            if (orderBody.PromoCode != null)
            {
                var result = await _promoCodeRepository.ActivePromocode(orderBody.PromoCode);
                if (result == false)
                    return BadRequest();
            }

            order = await _orderRepository.GetAsync(order.Id);
            var userInfos = await _telegramBotService.GetChatIdsAsync();
            var admins = await _userRepository.GetAllOrUpdateByChatId(userInfos);
            var adminChatIds = admins.Where(e => e.ChatId != null).Select(e => (long)e.ChatId);

            var tasks = new List<Task<bool>>();
            var orderMessage = CreateOrderMessage(order, orderBody.PromoCode);
            foreach (var email in admins.Select(e => e.Email))
            {
                var task = _emailService.SendMessage(email, "iStore", orderMessage);
                tasks.Add(task);
            }


            await _telegramBotService.SendMessageAsync(orderMessage, adminChatIds);
            await Task.WhenAll(tasks);
            return order == null ? BadRequest() : Ok();
        }

        [HttpPost("analytic")]
        [SwaggerOperation("Получить аналитику по заказам")]
        [SwaggerResponse(200, Type = typeof(AnalyticBody))]

        public async Task<IActionResult> GetAnalyticBody()
        {
            var result = await _orderRepository.GetAnalyticBody();
            return Ok(result);
        }

        private string CreateOrderMessage(Order order, string? promocode = null)
        {
            var buyerInfo = new StringBuilder();
            buyerInfo.AppendLine($"Покупатель - {order.Fullname}");
            buyerInfo.AppendLine($"Телефон - {order.Phone}");
            if (order.Email != null)
                buyerInfo.AppendLine($"Email - {order.Email}");

            buyerInfo.AppendLine($"Способ связи - {order.CommunicationMethod}");
            if (!string.IsNullOrEmpty(order.Comment))
                buyerInfo.AppendLine($"Комментарий - {order.Comment}");

            buyerInfo.AppendLine("\nТовары:\n");
            var productsInfo = order.Products.Select(e => e.ToString());
            foreach (var productInfo in productsInfo)
                buyerInfo.AppendLine(productInfo);

            buyerInfo.AppendLine($"Итоговая цена: {order.TotalSum} rub");
            if (promocode != null)
                buyerInfo.AppendLine($"Использованный промокод: {promocode}");

            return buyerInfo.ToString();
        }
    }
}