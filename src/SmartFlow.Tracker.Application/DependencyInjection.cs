namespace SmartFlow.Tracker.Application
{
    using Microsoft.Extensions.DependencyInjection;
    using SmartFlow.Tracker.Application.AI;
    using SmartFlow.Tracker.Application.Tasks;
    using SmartFlow.Tracker.Application.Tasks.Interfaces;


    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITasksService, TasksService>();
            
            return services;
        }
    }
}

