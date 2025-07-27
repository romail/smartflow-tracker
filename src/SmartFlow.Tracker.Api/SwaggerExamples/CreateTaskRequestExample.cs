namespace SmartFlow.Tracker.Api.SwaggerExamples
{
    using Swashbuckle.AspNetCore.Filters;
    using SmartFlow.Tracker.Application.Tasks.Models;

    public class CreateTaskRequestExample : IExamplesProvider<CreateTaskRequest>
    {
        public CreateTaskRequest GetExamples() =>
            new CreateTaskRequest("Build backend API with OpenAI integration", true);
    }
}