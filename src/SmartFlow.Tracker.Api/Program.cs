using Microsoft.OpenApi.Models;
using SmartFlow.Tracker.AI;
using SmartFlow.Tracker.Application;
using SmartFlow.Tracker.Infrastructure;
using Swashbuckle.AspNetCore.Filters;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc( "v1", new OpenApiInfo {   Title = "SmartFlow API", Version = "v1" });
    c.EnableAnnotations();
    // c.ExampleFilters(); 
});

// TODO: DI, DB, Application Layer
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAIServices();
builder.Services.AddBots();
builder.WebHost.UseUrls("http://*:80");

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFlow API v1");
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();