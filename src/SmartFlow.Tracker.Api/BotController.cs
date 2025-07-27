namespace SmartFlow.Tracker.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SmartFlow.Tracker.Application.Bots.Interfaces;

    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        private readonly ITelegramBotService _botService;

        public BotController(ITelegramBotService botService)
        {
            _botService = botService;
        }

        [HttpPost]
        public async Task<IActionResult> Receive([FromBody] object update, CancellationToken cancellationToken)
        {
            await _botService.HandleUpdateAsync(update.ToString()!, cancellationToken);
            return Ok();
        }
    }
}