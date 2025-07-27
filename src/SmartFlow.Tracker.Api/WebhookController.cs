using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SmartFlow.Tracker.Api.Controllers;

[ApiController]
[Route("api/bot")]
public class WebhookController : ControllerBase
{
    private readonly ITelegramBotClient _bot;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(ITelegramBotClient bot, ILogger<WebhookController> logger)
    {
        _bot = bot;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message?.Text is not { } messageText)
            return Ok();

        var chatId = update.Message.Chat.Id;
        _logger.LogInformation("📨 Message from {User}: {Text}", update.Message.From?.Username, messageText);

        string response = messageText.ToLower().Trim() switch
        {
            "/start" => "👋 Hello! I'm SmartFlow bot.",
            _ => $"You wrote: *{messageText}* _(currently only /start is handled)_"
        };

        await _bot.SendRequest(new SendMessageRequest
        {
            ChatId = chatId,
            Text = response,
            ParseMode = ParseMode.Markdown
        }, cancellationToken);

        return Ok();
    }
}