namespace SmartFlow.Tracker.Application.Bots.Interfaces
{
    public interface ITelegramBotService
    {
        Task HandleUpdateAsync(string jsonUpdate, CancellationToken cancellationToken = default);
    }
}