namespace SmartFlow.Tracker.Domain.Interfaces
{
    using SmartFlow.Tracker.Domain;

    public interface ITasksRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TaskEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(TaskEntity task, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(TaskEntity task, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}

