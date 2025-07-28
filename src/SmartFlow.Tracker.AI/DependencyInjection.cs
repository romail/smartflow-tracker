namespace SmartFlow.Tracker.AI
{
    using Microsoft.Extensions.DependencyInjection;
    using SmartFlow.Tracker.AI.Services;
    using SmartFlow.Tracker.Application.AI;
    using SmartFlow.Tracker.Application.AI.Interfaces;
    using SmartFlow.Tracker.Application.Bots.Interfaces;
    using SmartFlow.Tracker.Bots;  
    
    public static class DependencyInjection
    {
        public static IServiceCollection AddAIServices(this IServiceCollection services)
        {
            services.AddScoped<IAIService, OpenAIService>();
            return services;
        }
    }
}

