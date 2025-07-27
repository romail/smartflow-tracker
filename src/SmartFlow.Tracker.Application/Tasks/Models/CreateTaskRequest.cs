namespace SmartFlow.Tracker.Application.Tasks.Models
{
    public record CreateTaskRequest(string? Title, bool UseAI = false);
}