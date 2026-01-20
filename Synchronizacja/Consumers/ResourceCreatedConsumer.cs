using Core;
using Core.Events;
using MassTransit;
using Project.SyncService.Data;

namespace Project.SyncService.Consumers
{
    public class ResourceCreatedConsumer : IConsumer<ResourceCreated>
    {
        private readonly SyncDbContext _dbContext;
        private readonly ILogger<ResourceCreatedConsumer> _logger;

        public ResourceCreatedConsumer(SyncDbContext dbContext, ILogger<ResourceCreatedConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ResourceCreated> context)
        {
            var message = context.Message;
            _logger.LogInformation($"[SyncService] Odebrano nowy zasób: {message.Name}");

            var exists = await _dbContext.Resources.FindAsync(message.Id);
            if (exists == null)
            {
                var newResource = new Resource
                {
                    Id = message.Id,
                    Name = message.Name,
                    Description = message.Description,
                    CreatedAt = message.CreatedAt
                };

                _dbContext.Resources.Add(newResource);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("[SyncService] Zreplikowano do lokalnej bazy.");
            }
        }
    }
}