using MassTransit;
using Microsoft.EntityFrameworkCore;
using Project.SyncService.Consumers;
using Project.SyncService.Data;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<SyncDbContext>(options =>
            options.UseSqlServer("Server=localhost,1433;Database=DistributedDb;User Id=sa;Password=haslo123;TrustServerCertificate=True;"));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ResourceCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

    })
    .Build();

await host.RunAsync();