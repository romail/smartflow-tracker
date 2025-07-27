namespace SmartFlow.Tracker.Domain;

public class TaskEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Status { get; set; } = "Todo"; // Todo, InProgress, Done
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}