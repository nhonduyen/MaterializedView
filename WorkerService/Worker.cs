using Microsoft.EntityFrameworkCore;
using System.Threading;
using WorkerService.Data;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
				try
				{
					_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

					using var scope = _serviceProvider.CreateScope();
					var context = scope.ServiceProvider.GetService<MaterializedContext>();

					var sql = @"
SET IDENTITY_INSERT [Dest].[WidgetLatestState] ON;
MERGE [Dest].[WidgetLatestState] AS a
 USING (
 SELECT
   v.[WidgetID]
	, v.[LastTripID]
	, v.[LastEventDate]
	, v.[ArrivalDate]
	, v.[DepartureDate]
 FROM
   [Dest].[uv_WidgetLatestState] v
 ) AS T
 ON
 (
   a.[WidgetID] = t.[WidgetID]
 )

WHEN MATCHED AND t.ArrivalDate IS NOT NULL THEN
     UPDATE
      SET LastTripID = t.LastTripID
	, LastEventDate = t.LastEventDate
	, ArrivalDate = t.ArrivalDate
	, DepartureDate = t.DepartureDate

WHEN NOT MATCHED BY TARGET AND t.ArrivalDate IS NOT NULL THEN
      INSERT (
        WidgetID
	, LastTripID
	, LastEventDate
	, ArrivalDate
	, DepartureDate
      ) VALUES (
        t.[WidgetID]
	, t.[LastTripID]
	, t.[LastEventDate]
	, t.[ArrivalDate]
	, t.[DepartureDate]
      )

WHEN MATCHED AND t.ArrivalDate IS NULL THEN
     DELETE;

SET IDENTITY_INSERT [Dest].[WidgetLatestState] OFF;
";

					var result = await context.Database.ExecuteSqlRawAsync(sql, stoppingToken);

					_logger.LogInformation($"{result} items have been updated");
				} catch (Exception ex)
				{
					_logger.LogError($"{ex.Message}");
					throw ex;
				}
                
                await Task.Delay(1000*10, stoppingToken);
            }
        }
    }
}