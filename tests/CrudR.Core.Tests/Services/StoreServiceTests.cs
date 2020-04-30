using System;
using System.Text.Json;
using System.Threading.Tasks;
using CrudR.Core.Models;
using CrudR.Core.Repositories;
using CrudR.Core.Services;
using CrudR.Core.Validators;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrudR.Core.Tests.Services
{
    public class StoreServiceTests
    {
        public class TheCreateStoreMethod
        {
            [Fact]
            public async Task ShouldCallCreateAsyncWithStoreModel_WhenStoreModelIsValid()
            {
                // Arrange
                var storeModel = new StoreModel("/test", new JsonElement());
                var storeRepositoryMock = GetStoreRepositoryMock();
                var storeModelValidatorMock = GetStoreModelValidatorMock();

                var storeService = new StoreService(storeRepositoryMock.Object, storeModelValidatorMock.Object);

                // Act
                await storeService.CreateStoreAsync(storeModel, default);

                // Assert
                storeRepositoryMock.Verify(repo => repo.CreateAsync(storeModel, default), Times.Once);
            }

            [Fact]
            public async Task ShouldCallCreateAsyncWithNullStoreModel_WhenStoreModelIsNull()
            {
                // Arrange
                StoreModel storeModel = null;
                var storeRepositoryMock = GetStoreRepositoryMock();
                var storeModelValidatorMock = GetStoreModelValidatorMock();

                var storeService = new StoreService(storeRepositoryMock.Object, storeModelValidatorMock.Object);

                // Act
                await storeService.CreateStoreAsync(storeModel, default);

                // Assert
                storeRepositoryMock.Verify(repo => repo.CreateAsync(storeModel, default), Times.Once);
            }
        }

        public class TheReadStoreMethod
        {
            [Fact]
            public async Task ShouldReturnStoreModel_WhenGivenId()
            {
                // Arrange
                const string id = "/test";
                var storeModel = new StoreModel(id, new JsonElement());

                var storeRepositoryMock = GetStoreRepositoryMock();
                storeRepositoryMock.Setup(repo => repo.ReadAsync(It.IsAny<string>(), default))
                    .Returns(Task.FromResult(storeModel));
                var storeModelValidatorMock = GetStoreModelValidatorMock();

                var storeService = new StoreService(storeRepositoryMock.Object, storeModelValidatorMock.Object);

                // Act
                var result = await storeService.ReadStoreAsync(id, default);

                // Assert
                result.Should().Be(storeModel);
            }
        }

        public class TheUpdateStoreMethod
        {
            [Fact]
            public void ShouldThrowsArgumentNullException_WhenStoreModelIsNull()
            {
                // Arrange
                StoreModel storeModel = null;
                var storeRepositoryMock = GetStoreRepositoryMock();
                var storeModelValidatorMock = GetStoreModelValidatorMock();

                var storeService = new StoreService(storeRepositoryMock.Object, storeModelValidatorMock.Object);

                // Act
                Func<Task> result = async () => await storeService.UpdateStoreAsync(storeModel, default);

                // Assert
                result.Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("storeModel");
            }

            [Fact]
            public async Task ShouldCallValidateWithStoreModelAndStoredStoreModel_WhenStoreModelIsValid()
            {
                // Arrange
                var storeModel = new StoreModel("/test", new JsonElement());
                var storedStoreModel = new StoreModel("/stored", new JsonElement());

                var storeRepositoryMock = GetStoreRepositoryMock();
                storeRepositoryMock.Setup(repo => repo.ReadAsync(storeModel.Id, default))
                    .Returns(Task.FromResult(storedStoreModel));
                var storeModelValidatorMock = GetStoreModelValidatorMock();

                var storeService = new StoreService(storeRepositoryMock.Object, storeModelValidatorMock.Object);

                // Act
                await storeService.UpdateStoreAsync(storeModel, default);

                // Assert
                storeModelValidatorMock.Verify(validator => validator.Validate(storeModel, storedStoreModel), Times.Once);
            }

            [Fact]
            public async Task ShouldCallUpdateAsyncWithStoreModel_WhenStoreModelIsValid()
            {
                // Arrange
                const string id = "/test";
                var storeModel = new StoreModel(id, new JsonElement());

                var storeRepositoryMock = GetStoreRepositoryMock();
                var storeModelValidatorMock = GetStoreModelValidatorMock();

                var storeService = new StoreService(storeRepositoryMock.Object, storeModelValidatorMock.Object);

                // Act
                await storeService.UpdateStoreAsync(storeModel, default);

                // Assert
                storeRepositoryMock.Verify(repo => repo.UpdateAsync(storeModel, default), Times.Once);
            }
        }

        public class TheDeleteStoreMethod
        {
            [Fact]
            public async Task ShouldCallDeleteAsyncWithId_WhenGivenId()
            {
                // Arrange
                const string id = "/test";
                var storeRepositoryMock = GetStoreRepositoryMock();
                var storeModelValidatorMock = GetStoreModelValidatorMock();

                var storeService = new StoreService(storeRepositoryMock.Object, storeModelValidatorMock.Object);

                // Act
                await storeService.DeleteStoreAsync(id, default);

                // Assert
                storeRepositoryMock.Verify(repo => repo.DeleteAsync(id, default), Times.Once);
            }
        }

        private static Mock<IStoreRepository> GetStoreRepositoryMock()
            => new Mock<IStoreRepository>();

        private static Mock<IStoreModelValidator> GetStoreModelValidatorMock()
        {
            var mock = new Mock<IStoreModelValidator>();
            mock.Setup(validator => validator.Validate(It.IsNotNull<StoreModel>(), It.IsNotNull<StoreModel>()));

            return mock;
        }
    }
}
