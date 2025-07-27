namespace SmartFlow.Tracker.Application.Tasks
{
    using SmartFlow.Tracker.Application.AI.Interfaces;
    using SmartFlow.Tracker.Application.Tasks.Interfaces;
    using SmartFlow.Tracker.Application.Tasks.Models;
    using SmartFlow.Tracker.Domain;
    using SmartFlow.Tracker.Domain.Interfaces;
    
    public class TasksService(ITasksRepository repository, IAIService aiService) : ITasksService
    {
        public async Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var tasks = await repository.GetAllAsync(cancellationToken);
            return tasks.Select(t =>
                new TaskDto(
                    t.Id,
                    t.Title ?? string.Empty,
                    t.Status ?? "Todo",
                    t.CreatedAt
                ));
        }

        public async Task<TaskDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var task = await repository.GetByIdAsync(id, cancellationToken);

            return task is null
                ? null
                : new TaskDto(
                    task.Id,
                    task.Title ?? string.Empty,
                    task.Status ?? "Todo",
                    task.CreatedAt
                );
        }

        public async Task<int> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
        {
            if (request.UseAI && !string.IsNullOrWhiteSpace(request.Title))
            {
                var generated = await aiService.GenerateTasksAsync(request.Title, cancellationToken);

                foreach (var taskTitle in generated)
                {
                    var entity = new TaskEntity
                    {
                        Title = taskTitle,
                        Status = "Todo",
                        CreatedAt = DateTime.UtcNow
                    };

                    await repository.CreateAsync(entity, cancellationToken);
                }

                return -1; // спец-код — задачи созданы пакетно
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException("Title is required if AI generation is not used.");
            }

            var taskEntity = new TaskEntity()
            {
                Title = request.Title,
                Status = "Todo",
                CreatedAt = DateTime.UtcNow
            };

            return await repository.CreateAsync(taskEntity, cancellationToken);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaskRequest request, CancellationToken cancellationToken = default)
        {
            var task = await repository.GetByIdAsync(id, cancellationToken);
            if (task is null) return false;

            task.Title = request.Title ?? task.Title;
            task.Status = request.Status ?? task.Status;

            return await repository.UpdateAsync(task, cancellationToken);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
            => repository.DeleteAsync(id, cancellationToken);
    }
}
