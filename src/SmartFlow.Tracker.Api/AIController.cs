using Microsoft.AspNetCore.Mvc;
using SmartFlow.Tracker.Application.AI;
using SmartFlow.Tracker.Application.AI.Interfaces;

namespace SmartFlow.Tracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly IAIService _aiService;

    public AIController(IAIService ai)
    {
        _aiService = ai;
    }

    // [HttpPost("generate-tasks")]
    // public async Task<IActionResult> Generate([FromBody] PromptInput input)
    // {
    //     var tasks = await _ai.GenerateTasksFromPrompt(input.Input);
    //     return Ok(tasks);
    // }
    //
    // [HttpGet("summary")]
    // public async Task<IActionResult> Summary()
    // {
    //     var summary = await _ai.GetSummary();
    //     return Ok(summary);
    // }
    
    [HttpPost("generate-tasks")]
    public async Task<ActionResult<IReadOnlyCollection<string>>> GenerateTasks([FromBody] GenerateTasksRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Description))
            return BadRequest("Description is required.");

        var result = await _aiService.GenerateTasksAsync(request.Description, cancellationToken);
        return Ok(result);
    }

    public record GenerateTasksRequest(string Description);
}

public class PromptInput
{
    public string Input { get; set; } = string.Empty;
}