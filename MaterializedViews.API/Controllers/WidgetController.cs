using MaterializedViews.API.Infrastructrure.Data;
using MaterializedViews.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MaterializedViews.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MaterializedViews.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetController : ControllerBase
    {
        private readonly MaterializedContext _context;
        private readonly ILogger<WidgetController> _logger;

        public WidgetController(MaterializedContext context, ILogger<WidgetController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Insert(int quantity, CancellationToken cancellationToken)
        {
            var random = new Random();
            var events = new List<Event>(quantity);
            for (var i = 0; i < quantity; i++)
            {
                var e = new Event()
                {
                    WidgetID = random.Next(1, 100),
                    EventTypeID = random.Next(1, 4),
                    TripID = random.Next(1, 10),
                    EventDate = DateTime.UtcNow    
                };
                events.Add(e);
            }

            _context.Event.AddRange(events);
            var result = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"{ result } event have been added");
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> Get(CancellationToken cancellationToken)
        {
            var widgetLatestState = await _context.WidgetLatestState.AsNoTracking().ToListAsync(cancellationToken);
            return Ok(widgetLatestState);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(CancellationToken cancellationToken)
        {
            var result = await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Source.Event;", cancellationToken);
            _logger.LogInformation($"Table {nameof(Event)}'s data has been deleted");
            return Ok(result);
        }
    }
}
