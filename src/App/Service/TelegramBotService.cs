using istore_api.src.App.IService;
using istore_api.src.Domain.Entities.Config;
using istore_api.src.Domain.Entities.Shared;
using Newtonsoft.Json.Linq;


namespace istore_api.src.App.Service
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly TelegramBotSettings _botSettings;
        private readonly ILogger<TelegramBotService> _logger;
        private long _maxUpdateId = 0;

        public TelegramBotService(
            TelegramBotSettings botSettings,
            ILogger<TelegramBotService> logger
        )
        {
            _botSettings = botSettings;
            _logger = logger;
        }


        public async Task<List<TelegramBotUserInfo>> GetChatIdsAsync()
        {
            var userInfos = new List<TelegramBotUserInfo>();

            try
            {
                using var client = new HttpClient();
                string apiUrl = $"https://api.telegram.org/bot{_botSettings.Token}/getUpdates";
                var response = await client.GetStringAsync(apiUrl);

                var jsonResponse = JObject.Parse(response);
                var resultsToken = jsonResponse.SelectToken("result");

                if (resultsToken != null)
                {
                    foreach (var result in resultsToken)
                    {
                        var updateIdToken = result.SelectToken("update_id");
                        long updateId = updateIdToken != null ? updateIdToken.Value<long>() : 0;

                        var chatIdToken = result.SelectToken("message.chat.id");
                        long chatId = chatIdToken != null ? chatIdToken.Value<long>() : 0;

                        var textToken = result.SelectToken("message.text");
                        string? text = textToken != null ? textToken.Value<string>() : string.Empty;


                        if(_maxUpdateId < updateId)
                            _maxUpdateId = updateId;

                        if(updateId != 0 && chatId != 0 && text != string.Empty)
                        {
                            var emailWithPassword = text.Split(":");
                            if(emailWithPassword.Length == 2)
                            {
                                var userInfo = new TelegramBotUserInfo
                                {
                                    ChatId = chatId,
                                    Email = emailWithPassword[0],
                                    Password = emailWithPassword[1]
                                };

                                userInfos.Add(userInfo);
                            }
                        }
                    }
                }
            }catch(Exception e)
            {
                _logger.LogInformation(e.Message);
            }
            userInfos = userInfos.GroupBy(e => e.ChatId).Select(group => group.First()).ToList();
            return userInfos;
        }

        public async Task SendMessageAsync(string message, IEnumerable<long> chatIds)
        {
            var ids = chatIds.ToHashSet();
            if(ids.Count == 0)
            {
                await Task.CompletedTask;
                return;
            }

            string apiUrl = $"https://api.telegram.org/bot{_botSettings.Token}/sendMessage";            

            try
            {
                foreach(var chatId in ids)
                {
                    using HttpClient client = new();
                    var content = new FormUrlEncodedContent(new[]
                    {
                            new KeyValuePair<string, string>("chat_id", chatId.ToString()),
                            new KeyValuePair<string, string>("text", message)
                    });

                    var response = await client.PostAsync(apiUrl, content);
                    if(!response.IsSuccessStatusCode)
                        _logger.LogInformation($"Telegram send failed: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }catch(Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
    }
}