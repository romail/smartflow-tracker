
namespace SmartFlow.Tracker.Application.Bots.Interfaces
{
    using Telegram.Bot.Types;

    public interface ITelegramBotService
    {
        Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
    }
}