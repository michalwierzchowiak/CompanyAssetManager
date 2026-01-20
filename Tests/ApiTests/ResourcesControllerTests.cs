using API.Controllers;
using API.Data;
using API.Hubs;
using Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Tests.ApiTests
{
    public class ResourcesControllerTests
    {
        [Fact]
        public async Task GetResource_ReturnsResource_WhenExists()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
            var context = new AppDbContext(options);

            var testId = Guid.NewGuid();
            context.Resources.Add(new Resource { Id = testId, Name = "Test Resource" });
            await context.SaveChangesAsync();

            var mockPublishEndpoint = new Mock<IPublishEndpoint>();
            var mockHubContext = new Mock<IHubContext<NotificationsHub>>();

            var controller = new ResourcesController(context, mockPublishEndpoint.Object, mockHubContext.Object);

            var result = await controller.GetResource(testId);

            var resource = Assert.IsType<Resource>(result.Value);
            Assert.Equal("Test Resource", resource.Name);
            Assert.Equal(testId, resource.Id);
        }

        [Fact]
        public async Task GetResource_ReturnsNotFound_WhenDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);

            var mockPublishEndpoint = new Mock<IPublishEndpoint>();
            var mockHubContext = new Mock<IHubContext<NotificationsHub>>();

            var controller = new ResourcesController(context, mockPublishEndpoint.Object, mockHubContext.Object);

            var result = await controller.GetResource(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}