namespace SmartFlow.Tracker.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using SmartFlow.Tracker.Application.Tasks.Interfaces;
    using SmartFlow.Tracker.Application.Tasks.Models;
    using SmartFlow.Tracker.Api.SwaggerExamples;
    using Swashbuckle.AspNetCore.Filters;

    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all tasks", Description = "Returns a list of all tasks in the system")]
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _tasksService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get task by ID", Description = "Returns a task by its ID")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var task = await _tasksService.GetByIdAsync(id, cancellationToken);
            return task is null ? NotFound() : Ok(task);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a task", Description = "Creates a task manually or via AI if UseAI = true")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [SwaggerRequestExample(typeof(CreateTaskRequest), typeof(CreateTaskRequestExample))]
        public async Task<ActionResult<int>> Create([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newId = await _tasksService.CreateAsync(request, cancellationToken);

            if (newId == -1)
                return Ok("Tasks generated via AI and saved.");

            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update task", Description = "Updates a task's title or status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _tasksService.UpdateAsync(id, request, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete task", Description = "Deletes a task by ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var success = await _tasksService.DeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }
    }
}
