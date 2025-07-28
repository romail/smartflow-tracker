namespace SmartFlow.Tracker.Api.Controllers
{
    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Microsoft.AspNetCore.Mvc;
    using SmartFlow.Tracker.Application.Bots.Interfaces;

    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        private readonly ITelegramBotService _botService;
        private readonly ITelegramBotClient _bot;
        private readonly IConfiguration _config;

        public BotController(ITelegramBotService botService, IConfiguration config, ITelegramBotClient bot)
        {
            _botService = botService;
            _config = config;
            _bot = bot;
        }

        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] Update update, CancellationToken cancellationToken)
        {
            await _botService.HandleUpdateAsync(update, cancellationToken);
            return Ok();
        }
        
        [HttpGet("set-webhook")]
        public async Task<IActionResult> SetWebhook()
        {
            var url = _config["Telegram:WebhookUrl"];
            await _bot.SetWebhook(url);
            
            return Ok($"✅ Webhook set: {url}");
        }
    }
}