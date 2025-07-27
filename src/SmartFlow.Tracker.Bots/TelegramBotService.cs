namespace SmartFlow.Tracker.Bots
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Telegram.Bot;
    using Telegram.Bot.Polling;
    using Telegram.Bot.Requests;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Exceptions;


    public class TelegramBotService(ILogger<TelegramBotService> logger, IConfiguration configuration)
        : BackgroundService
    {
        private ITelegramBotClient? _bot;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var token = configuration["Telegram:BotToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                logger.LogError("❌ Telegram bot token is missing in configuration");
                return;
            }

            _bot = new TelegramBotClient(token);

            var me = await _bot.SendRequest(new GetMeRequest(), stoppingToken);
            logger.LogInformation("🤖 Connected as @{Username}", me.Username);

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _bot.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                cancellationToken: stoppingToken
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Message?.Text is not { } messageText)
                return;

            var chatId = update.Message.Chat.Id;
            logger.LogInformation("📩 Message from {User}: {Text}", update.Message.From?.Username, messageText);

            string reply = messageText.Trim().ToLower() switch
            {
                "/start" => "👋 Hello! I'm SmartFlow bot. Ready to manage your tasks.",
                _ => $"You said: *{messageText}*\n_(but I only understand /start for now)_"
            };

            await bot.SendRequest(
                new SendMessageRequest
                {
                    ChatId = chatId,
                    Text = reply,
                    ParseMode = ParseMode.Markdown
                },
                cancellationToken
            );
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception,
            CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiEx => $"Telegram API Error [{apiEx.ErrorCode}]: {apiEx.Message}",
                _ => exception.ToString()
            };

            logger.LogError("❗ Bot error: {Error}", errorMessage);
            return Task.CompletedTask;
        }
    }
}
