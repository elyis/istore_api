using istore_api.src.App.IService;
using istore_api.src.Domain.Entities.Request;
using istore_api.src.Domain.Entities.Shared;
using istore_api.src.Domain.Enums;
using istore_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace istore_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITelegramBotService _telegramBotService;
        private readonly IUserRepository _userRepository;


        public AuthController(
            IAuthService authService,
            ITelegramBotService telegramBotService,
            IUserRepository userRepository
        )
        {
            _authService = authService;
            _telegramBotService = telegramBotService;
            _userRepository = userRepository;
        }


        [SwaggerOperation("Регистрация")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPair))]
        [SwaggerResponse(400, "Токен не валиден или активирован")]
        [SwaggerResponse(409, "Почта уже существует")]


        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync(SignUpBody signUpBody)
        {
            string role = Enum.GetName(UserRole.Admin)!;
            var result = await _authService.SignUp(signUpBody, role);
            return result;
        }

        [SwaggerOperation("Добавить аккаунт пользователя")]
        [SwaggerResponse(200)]
        [HttpPost("tg")]
        public async Task<IActionResult> TgRegistration()
        {
            var userInfos = await _telegramBotService.GetChatIdsAsync();
            var admins = await _userRepository.GetAllOrUpdateByChatId(userInfos);
            return Ok();
        }



        [SwaggerOperation("Авторизация")]
        [SwaggerResponse(200, "Успешно", Type = typeof(TokenPair))]
        [SwaggerResponse(400, "Пароли не совпадают")]
        [SwaggerResponse(404, "Email не зарегистрирован")]

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(SignInBody signInBody)
        {
            var result = await _authService.SignIn(signInBody);
            return result;
        }

        [SwaggerOperation("Восстановление токена")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPair))]
        [SwaggerResponse(404, "Токен не используется")]

        [HttpPost("token")]
        public async Task<IActionResult> RestoreTokenAsync(TokenBody body)
        {
            var result = await _authService.RestoreToken(body.Value);
            return result;
        }
    }
}