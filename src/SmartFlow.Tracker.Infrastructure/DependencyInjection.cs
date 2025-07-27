namespace SmartFlow.Tracker.Infrastructure
{
    using SmartFlow.Tracker.Application.Tasks;
    using SmartFlow.Tracker.Application.Tasks.Interfaces;
    using SmartFlow.Tracker.Domain.Interfaces;
    using SmartFlow.Tracker.Infrastructure.Repositories;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Npgsql;
    using System.Data;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            var connStr = config.GetConnectionString("DefaultConnection");
            services.AddScoped<ITasksRepository, TasksRepository>();
            services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connStr));

            return services;
        }
    }
}

