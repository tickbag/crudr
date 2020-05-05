using System.Text.Json;
using System.Threading.Tasks;
using CrudR.Api.Controllers;
using CrudR.Api.Models;
using CrudR.Core.Models;
using CrudR.Core.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CrudR.Api.Tests.Controllers
{
    public class StoreControllerTests
    {
        public class TheGetAsyncMethod
        {
            [Fact]
            public async Task ShouldReturnJsonPayload_WhenGivenAUri()
            {
                // Arrange
                var uri = "/test";
                var storeModel = new StoreModel(uri, new JsonElement());

                var storeServiceMock = new Mock<IStoreService>();
                storeServiceMock.Setup(service => service.ReadStoreAsync(uri, default))
                    .Returns(Task.FromResult(storeModel));

                var controller = new StoreController(storeServiceMock.Object);

                // Act
                var result = await controller.GetAsync(uri, default);

                // Assert
                result.Should().Be(storeModel.Payload);
            }

            [Fact]
            public async Task ShouldReturnNullIfNullInternalModel_WhenGivenAnId()
            {
                // Arrange
                var id = "/test";
                StoreModel storeModel = null;

                var storeServiceMock = new Mock<IStoreService>();
                storeServiceMock.Setup(service => service.ReadStoreAsync(id, default))
                    .Returns(Task.FromResult(storeModel));

                var controller = new StoreController(storeServiceMock.Object);

                // Act
                var result = await controller.GetAsync(id, default);

                // Assert
                result.Should().BeNull();
            }
        }

        public class ThePostAsyncMethod
        {
            [Fact]
            public async Task ShouldReturnCreatedAtActionResponse_WhenGivenAUriAndJsonElement()
            {
                // Arrange
                var uri = "/test";
                var postResponse = new PostResponse(uri);
                var payload = new JsonElement();

                var storeServiceMock = new Mock<IStoreService>();
                storeServiceMock.Setup(service => service.CreateStoreAsync(It.IsAny<StoreModel>(), default));

                var controller = new StoreController(storeServiceMock.Object);

                // Act
                var result = await controller.PostAsync(uri, payload, default);
                var createdResult = result as CreatedAtActionResult;

                // Assert
                createdResult.Should().NotBeNull();
                createdResult.Value.Should().BeEquivalentTo(postResponse);
            }

            [Fact]
            public async Task ShouldCallCreateStoreAsync_WhenGivenAUriAndJsonElement()
            {
                // Arrange
                var uri = "/test";
                var payload = new JsonElement();

                var storeServiceMock = new Mock<IStoreService>();
                storeServiceMock.Setup(service => service.CreateStoreAsync(It.IsAny<StoreModel>(), default));

                var controller = new StoreController(storeServiceMock.Object);

                // Act
                var result = await controller.PostAsync(uri, payload, default);
                var createdResult = result as CreatedResult;

                // Assert
                storeServiceMock.Verify(service => service.CreateStoreAsync(It.IsAny<StoreModel>(), default), Times.Once);
            }
        }

        public class ThePutAsyncMethod
        {
            [Fact]
            public async Task ShouldReturnOkResponse_WhenGivenAUriAndJsonElement()
            {
                // Arrange
                var uri = "/test";
                var payload = new JsonElement();

                var storeServiceMock = new Mock<IStoreService>();
                storeServiceMock.Setup(service => service.UpdateStoreAsync(It.IsAny<StoreModel>(), default));

                var controller = new StoreController(storeServiceMock.Object);

                // Act
                var result = await controller.PutAsync(uri, payload, default);

                // Assert
                result.Should().BeOfType<OkResult>();
            }

            [Fact]
            public async Task ShouldCallUpdateStoreAsync_WhenGivenAUriAndJsonElement()
            {
                // Arrange
                var uri = "/test";
                var payload = new JsonElement();

                var storeServiceMock = new Mock<IStoreService>();
                storeServiceMock.Setup(service => service.UpdateStoreAsync(It.IsAny<StoreModel>(), default));

                var controller = new StoreController(storeServiceMock.Object);

                // Act
                var result = await controller.PutAsync(uri, payload, default);

                // Assert
                storeServiceMock.Verify(service => service.UpdateStoreAsync(It.IsAny<StoreModel>(), default), Times.Once);
            }
        }

        public class TheDeleteAsyncMethod
        {
            [Fact]
            public async Task ShouldCallDeleteStoreAsync_WhenGivenAUri()
            {
                // Arrange
                var uri = "/test";

                var storeServiceMock = new Mock<IStoreService>();
                storeServiceMock.Setup(service => service.DeleteStoreAsync(uri, default));

                var controller = new StoreController(storeServiceMock.Object);

                // Act
                await controller.DeleteAsync(uri, default);

                // Assert
                storeServiceMock.Verify(service => service.DeleteStoreAsync(uri, default), Times.Once);
            }
        }
    }
}
