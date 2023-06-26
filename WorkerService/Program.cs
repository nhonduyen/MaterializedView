using Microsoft.EntityFrameworkCore;
using WorkerService;
using WorkerService.Data;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration config = hostContext.Configuration;
        services.AddDbContext<MaterializedContext>(option =>
        {
            option.UseSqlServer(config.GetConnectionString("DefaultConnection"), providerOptions => providerOptions.CommandTimeout(120));
        });

        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync();
