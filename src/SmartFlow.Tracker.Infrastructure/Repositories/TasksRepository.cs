namespace SmartFlow.Tracker.Infrastructure.Repositories
{
    using Dapper;
    using System.Data;
    using SmartFlow.Tracker.Domain;
    using SmartFlow.Tracker.Domain.Interfaces;

    public class TasksRepository : ITasksRepository
    {
        private readonly IDbConnection _db;

        public TasksRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.QueryAsync<TaskEntity>("SELECT * FROM tasks ORDER BY id");
        }

        public async Task<TaskEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.QueryFirstOrDefaultAsync<TaskEntity>("SELECT * FROM tasks WHERE id = @id", new { id });
        }

        public async Task<int> CreateAsync(TaskEntity task, CancellationToken cancellationToken = default)
        {
            var sql = "INSERT INTO tasks (title, status, createdat) VALUES (@Title, @Status, @CreatedAt) RETURNING id";
            return await _db.ExecuteScalarAsync<int>(sql, task);
        }

        public async Task<bool> UpdateAsync(TaskEntity task, CancellationToken cancellationToken = default)
        {
            var sql = "UPDATE tasks SET title = @Title, status = @Status WHERE id = @Id";
            return await _db.ExecuteAsync(sql, task) > 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.ExecuteAsync("DELETE FROM tasks WHERE id = @id", new { id }) > 0;
        }
    }
}