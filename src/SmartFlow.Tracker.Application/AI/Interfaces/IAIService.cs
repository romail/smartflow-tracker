namespace SmartFlow.Tracker.Application.AI.Interfaces;

public interface IAIService
{
    Task<IReadOnlyCollection<string>> GenerateTasksAsync(string description, CancellationToken cancellationToken = default);
    // Task<List<string>> GenerateTasksFromPrompt(string input);
    // Task<string> GetSummary();
}