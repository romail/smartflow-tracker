namespace SmartFlow.Tracker.Application.Tasks.Interfaces
{
    using SmartFlow.Tracker.Application.Tasks.Models;

    public interface ITasksService
    {
        Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TaskDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, UpdateTaskRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }

   
}

