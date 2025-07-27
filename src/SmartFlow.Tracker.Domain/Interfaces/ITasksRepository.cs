namespace SmartFlow.Tracker.Infrastructure.Repositories.Interfaces
{
    using SmartFlow.Tracker.Domain;

    public interface ITasksRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync();
        Task<TaskEntity?> GetByIdAsync(int id);
        Task<int> CreateAsync(TaskEntity task);
        Task<bool> UpdateAsync(TaskEntity task);
        Task<bool> DeleteAsync(int id);
    }
}

