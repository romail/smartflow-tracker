namespace SmartFlow.Tracker.Application.Tasks.Models
{
    public record TaskDto(
        int Id,
        string Title,
        string Status,
        DateTime CreatedAt
    );
}