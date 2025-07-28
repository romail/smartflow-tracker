namespace SmartFlow.Tracker.Api.Controllers
{
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Microsoft.AspNetCore.Mvc;
    using Telegram.Bot.Types.Enums;

    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<BotController> _logger;

        public BotController(ITelegramBotClient botClient, ILogger<BotController> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message?.Text is string message)
            {
                var chatId = update.Message.Chat.Id;
                _logger.LogInformation("📨 Message from {user}: {text}", update.Message.From?.Username, message);

                var reply = message.Trim().ToLower() switch
                {
                    "/start" => "👋 Hello! I'm SmartFlow Bot",
                    _ => $"You said: {message}"
                };

                await _botClient.SendMessage(
                    chatId: chatId,
                    text: reply,
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken
                );
            }

            return Ok();
        }

        [HttpGet("set-webhook")]
        public async Task<IActionResult> SetWebhook([FromServices] IConfiguration config)
        {
            var webhookUrl = config["Telegram:WebhookUrl"];
            if (string.IsNullOrWhiteSpace(webhookUrl))
                return BadRequest("❌ Webhook URL is not configured");

            await _botClient.SetWebhook(webhookUrl);
            return Ok($"✅ Webhook set: {webhookUrl}");
        }
    }
}