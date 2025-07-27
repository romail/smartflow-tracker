namespace SmartFlow.Tracker.Bots
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Telegram.Bot;
    using Telegram.Bot.Exceptions;
    using Telegram.Bot.Polling;
    using Telegram.Bot.Requests;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    
    public class TelegramBotService(ILogger<TelegramBotService> logger, IConfiguration configuration)
        : BackgroundService
    {
        private TelegramBotClient? _botClient;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var token = configuration["Telegram:BotToken"];
            if (string.IsNullOrWhiteSpace(token))
            {
                logger.LogError("❌ Telegram bot token is missing in configuration");
                return;
            }

            _botClient = new TelegramBotClient(token);
            var me = await _botClient.SendRequest(new GetMeRequest(), stoppingToken);
            logger.LogInformation("🤖 Telegram bot connected as @{Username}", me.Username);

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>(), // All types
                ThrowPendingUpdates = true
            };

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandleErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken
            );

            // Блокируем задачу пока токен отмены не сработает
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message || update.Message?.Text is not { } messageText)
                return;

            var chatId = update.Message.Chat.Id;
            logger.LogInformation("📨 Received message from {User}: {Text}", update.Message.From?.Username,
                messageText);

            string reply;

            switch (messageText.Trim().ToLowerInvariant())
            {
                case "/start":
                    reply = "👋 Hello! I'm SmartFlow bot. Ready to manage your tasks.";
                    break;

                default:
                    reply = $"You said: *{messageText}*\n_(but I only understand `/start` for now)_";
                    break;
            }

            await bot.SendRequest(
                new SendMessageRequest
                {
                    ChatId = chatId,
                    Text = reply,
                    ParseMode = ParseMode.Markdown
                },
                cancellationToken);
        }

        private Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            string errorMsg = exception switch
            {
                ApiRequestException apiEx => $"Telegram API Error [{apiEx.ErrorCode}]: {apiEx.Message}",
                _ => exception.ToString()
            };

            logger.LogError("❗ Telegram Bot Error: {Error}", errorMsg);
            return Task.CompletedTask;
        }
    }
}
