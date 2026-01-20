using Core.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Synchronizacja.Consumers;
using Synchronizacja.Data;
using Xunit;

namespace Tests.SyncTests
{
    public class ConsumerTests
    {
        [Fact]
        public async Task Consume_ShouldAddResourceToDatabase()
        {

            var options = new DbContextOptionsBuilder<SyncDbContext>()
                .UseInMemoryDatabase(databaseName: "SyncTestDb")
                .Options;
            var context = new SyncDbContext(options);

            var mockLogger = new Mock<ILogger<ResourceCreatedConsumer>>();

            var consumer = new ResourceCreatedConsumer(context, mockLogger.Object);

            var mockContext = new Mock<ConsumeContext<ResourceCreated>>();
            var message = new ResourceCreated
            {
                Id = Guid.NewGuid(),
                Name = "Zsynchronizowany Zasób",
                CreatedAt = DateTime.Now
            };
            mockContext.Setup(x => x.Message).Returns(message);

            await consumer.Consume(mockContext.Object);

            var savedResource = await context.Resources.FirstOrDefaultAsync(r => r.Id == message.Id);
            Assert.NotNull(savedResource);
            Assert.Equal("Zsynchronizowany Zasób", savedResource.Name);
        }
    }
}