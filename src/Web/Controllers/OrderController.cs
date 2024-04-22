using System.Text;
using istore_api.src.App.IService;
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.IRepository;
using istore_api.src.Domain.Models;
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
        private readonly IInitialRegistrationRepository _initialRegistrationRepository;
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ITelegramBotService _telegramBotService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IPromoCodeRepository promoCodeRepository,
            IInitialRegistrationRepository initialRegistrationRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            ITelegramBotService telegramBotService,
            ILogger<OrderController> logger
        )
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _promoCodeRepository = promoCodeRepository;
            _initialRegistrationRepository = initialRegistrationRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _telegramBotService = telegramBotService;
            _logger = logger;
        }

        [HttpPost("order")]
        [SwaggerOperation("Создать заказ")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> CreateOrder(CreateOrderBody orderBody)
        {
            _logger.LogError("1");
            if (!orderBody.Configurations.Any())
                return BadRequest("empty configuration");


            _logger.LogError("2");
            var configIds = orderBody.Configurations.Select(e => e.ConfigurationId).ToHashSet();
            var configs = (await _productRepository.GetAll(configIds)).ToList();

            if (configs.Count != configIds.Count)
                return BadRequest();

            _logger.LogError("3");

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
            var orderMessage = CreateOrderMessage(order, orderBody.PromoCode);
            await SendNotification(orderMessage);
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

        [HttpPost("trade-in/request")]
        [SwaggerOperation("Отправить заявку на trade-in")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> TradeInRequest(TradeInRequestBody tradeInRequestBody)
        {


            var message = $"Заявка на trade-in\n\n" +
                          $"Телефон - {tradeInRequestBody.Phone}\n" +
                          $"Модель устройства - {tradeInRequestBody.DeviceModel}\n" +
                          $"Состояние корпуса - {tradeInRequestBody.CorpusState}\n" +
                          $"Состояние дисплея - {tradeInRequestBody.DisplayState}\n" +
                          $"Состояние батареи - {tradeInRequestBody.BatteryState}";

            await SendNotification(message);

            return Ok();
        }

        [HttpPost("discount")]
        [SwaggerOperation("Отправить заявку на скидку")]
        [SwaggerResponse(200, Type = typeof(PromoCodeBody))]
        [SwaggerResponse(409)]

        public async Task<IActionResult> RequestForDiscount(RequestForDiscountBody body)
        {
            var initialRegistration = await _initialRegistrationRepository.Create(body.Email);
            if (initialRegistration == null)
                return Conflict("The email is already used");

            var promocodeBody = new CreatePromoCodeBody
            {
                Type = PromoCodeType.DiscountPercentage,
                Value = 10,
            };

            var code = await _promoCodeRepository.AddAsync(promocodeBody);
            var promocode = code.ToPromoCodeBody().Code;

            var message = $"Промокод на скидку в 10%: {promocode}";
            await _emailService.SendMessage(body.Email, "Промокод", message);
            return Ok();
        }



        [HttpPost("request-for-change-cost")]
        [SwaggerOperation("Отправить заявку на изменение цены покупки")]
        [SwaggerResponse(200)]

        public async Task<IActionResult> RequestForChangeCost(RequestForChangeCostBody requestForChangeCostBody)
        {
            var message = $"Заявка на изменение цены покупки\n\n" +
                          $"Покупатель - {requestForChangeCostBody.Fullname}\n" +
                          $"Телефон - {requestForChangeCostBody.Phone}\n" +
                          $"Модель устройства - {requestForChangeCostBody.DeviceModel}\n" +
                          $"Ссылка на другой магазин - {requestForChangeCostBody.UrlToOtherStore}";

            await SendNotification(message);

            return Ok();
        }

        private async Task SendNotification(string message)
        {
            var userInfos = await _telegramBotService.GetChatIdsAsync();
            var admins = await _userRepository.GetAllOrUpdateByChatId(userInfos);
            var adminChatIds = admins.Where(e => e.ChatId != null).Select(e => (long)e.ChatId);

            var tasks = new List<Task<bool>>();
            foreach (var email in admins.Select(e => e.Email))
            {
                var task = _emailService.SendMessage(email, "iStore", message);
                tasks.Add(task);
            }


            await _telegramBotService.SendMessageAsync(message, adminChatIds);
            await Task.WhenAll(tasks);
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