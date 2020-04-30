using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CrudR.Context.Abstractions;
using CrudR.Core.Exceptions;
using CrudR.Core.Models;
using CrudR.DAL.Entities;
using CrudR.DAL.Integration;
using CrudR.DAL.Repositories;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace CrudR.DAL.Tests.Repositories
{
    public class StoreRepositoryTests
    {
        public class TheCreateAsyncMethod
        {
            private StoreEntity _storeEntity = null;

            [Fact]
            public void ShouldThrowArgumentNullException_WhenStoreModelisNull()
            {
                // Arrange
                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = new Mock<IDatabaseIntegrator<StoreEntity>>();

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                Func<Task> result = () => storeRepository.CreateAsync(null, default);

                // Aseert
                result.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("storeModel");
            }

            [Fact]
            public async Task ShouldCallInsertAsyncWithStoreModelData_WhenStoreModelIsValid()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(1);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                await storeRepository.CreateAsync(storeModel, default);

                // Aseert
                _storeEntity.Id.Should().Be(id);
                _storeEntity.Payload.ToString().Replace(" ", "").Should().BeEquivalentTo(payload.ToString().Replace(" ", ""));
            }

            [Fact]
            public async Task ShouldSetResponseRevisionInRevisionContext_WhenStoreModelIsValid()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(1);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                await storeRepository.CreateAsync(storeModel, default);

                // Aseert
                revisionContext.ResponseRevision.Should().Be(_storeEntity.Revision);
            }

            [Fact]
            public void ShouldThrowRecordAlreadyExistsException_WhenStoreModelIsValidAndIdIsAlreadyInUse()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(0);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                Func<Task> result = () => storeRepository.CreateAsync(storeModel, default);

                // Aseert
                result.Should().Throw<RecordAlreadyExistsException>();
            }

            [Fact]
            public async Task ShouldNotSetResponseRevisionInRevisionContext_WhenStoreModelIsValidAndIdIsAlreadyInUse()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(0);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                try
                {
                    await storeRepository.CreateAsync(storeModel, default);
                }
                catch { }

                // Aseert
                revisionContext.ResponseRevision.Should().BeNull();
            }

            private Mock<IDatabaseIntegrator<StoreEntity>> GetDataIntegratorMock(long modificationResult)
            {
                var databaseModificationResult = Task.FromResult(new DataModificationResult(modificationResult));

                var databaseIntegratorMock = new Mock<IDatabaseIntegrator<StoreEntity>>();
                databaseIntegratorMock.Setup(integrator => integrator.InsertAsync(It.IsAny<StoreEntity>(), default))
                    .Returns(databaseModificationResult)
                    .Callback<StoreEntity, CancellationToken>((entity, cancellationToken) => _storeEntity = entity);

                return databaseIntegratorMock;
            }
        }

        public class TheReadAsyncMethod
        {
            [Fact]
            public async Task ShouldReturnStoreModel_WhenGivenAnIdAndExistingStoreModel()
            {
                // Arrange
                var id = "/test";

                var json = "{ \"myValue\": \"test\" }";
                var payload = BsonDocument.Parse(json);
                var storeEntity = new StoreEntity
                {
                    Id = id,
                    Revision = Guid.NewGuid(),
                    Payload = payload
                };

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(storeEntity);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                var result = await storeRepository.ReadAsync(id, default);

                // Aseert
                result.Id.Should().Be(id);
                result.Payload.ToString().Replace(" ", "").Should().Be(json.Replace(" ", ""));
            }

            [Fact]
            public async Task ShouldSetResponseRevisionInRevisionContext_WhenGivenAnIdAndExistingStoreModel()
            {
                // Arrange
                var id = "/test";

                var json = "{ \"myValue\": \"test\" }";
                var payload = BsonDocument.Parse(json);
                var storeEntity = new StoreEntity
                {
                    Id = id,
                    Revision = Guid.NewGuid(),
                    Payload = payload
                };

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(storeEntity);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                var result = await storeRepository.ReadAsync(id, default);

                // Aseert
                revisionContext.ResponseRevision.Should().Be(storeEntity.Revision);
            }

            [Fact]
            public void ShouldThrowRecordNotFoundException_WhenGivenAnIdAndNoExistingStoreModel()
            {
                // Arrange
                var id = "/test";

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(null);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                Func<Task<StoreModel>> result = () => storeRepository.ReadAsync(id, default);

                // Aseert
                result.Should().Throw<RecordNotFoundException>();
            }

            [Fact]
            public async Task ShouldNotSetResponseRevisionInRevisionContext_WhenGivenAnIdAndNoExistingStoreModel()
            {
                // Arrange
                var id = "/test";

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(null);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                try
                {
                    var result = await storeRepository.ReadAsync(id, default);
                }
                catch { }

                // Assert
                revisionContext.ResponseRevision.Should().BeNull();
            }

            private Mock<IDatabaseIntegrator<StoreEntity>> GetDataIntegratorMock(StoreEntity returns)
            {
                var databaseIntegratorMock = new Mock<IDatabaseIntegrator<StoreEntity>>();
                databaseIntegratorMock.Setup(integrator => integrator.FindAsync(It.IsAny<string>(), default))
                    .Returns(Task.FromResult(returns));

                return databaseIntegratorMock;
            }
        }

        public class TheUpdateAsyncMethod
        {
            private StoreEntity _storeEntity = null;

            [Fact]
            public void ShouldThrowArgumentNullException_WhenStoreModelIsNull()
            {
                // Arrange
                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = new Mock<IDatabaseIntegrator<StoreEntity>>();

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                Func<Task> result = () => storeRepository.UpdateAsync(null, default);

                // Aseert
                result.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("storeModel");
            }

            [Fact]
            public async Task ShouldCallUpdateAsyncWithStoreModelData_WhenStoreModelIsValidWithNoRequestRevision()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(1);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                await storeRepository.UpdateAsync(storeModel, default);

                // Aseert
                _storeEntity.Id.Should().Be(id);
                _storeEntity.Payload.ToString().Replace(" ", "").Should().BeEquivalentTo(payload.ToString().Replace(" ", ""));

                databaseIntegratorMock.Verify(integrator => integrator.ReplaceAsync(id, It.IsAny<StoreEntity>(), default), Times.Once);
            }

            [Fact]
            public async Task ShouldCallUpdateAsyncWithStoreModelDataAndRequestRevision_WhenStoreModelIsValidWithRequestRevision()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext
                {
                    RequestRevision = Guid.NewGuid()
                };
                var databaseIntegratorMock = GetDataIntegratorMock(1);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                await storeRepository.UpdateAsync(storeModel, default);

                // Aseert
                _storeEntity.Id.Should().Be(id);
                _storeEntity.Payload.ToString().Replace(" ", "").Should().BeEquivalentTo(payload.ToString().Replace(" ", ""));

                databaseIntegratorMock.Verify(integrator => integrator.ReplaceAsync(id, revisionContext.RequestRevision, It.IsAny<StoreEntity>(), default), Times.Once);
            }

            [Fact]
            public async Task ShouldSetResponseRevisionInRevisionContext_WhenStoreModelIsValid()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(1);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                await storeRepository.UpdateAsync(storeModel, default);

                // Aseert
                revisionContext.ResponseRevision.Should().Be(_storeEntity.Revision);
            }

            [Fact]
            public void ShouldThrowRecordNotModifiedException_WhenStoreModelIsValidWithRequestRevisionAndNoMatchIsPresent()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext
                {
                    RequestRevision = Guid.NewGuid()
                };
                var databaseIntegratorMock = GetDataIntegratorMock(0);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                Func<Task> result = () => storeRepository.UpdateAsync(storeModel, default);

                // Aseert
                result.Should().Throw<RecordNotModifiedException>();

                databaseIntegratorMock.Verify(integrator => integrator.ReplaceAsync(id, revisionContext.RequestRevision, It.IsAny<StoreEntity>(), default), Times.Once);
            }

            [Fact]
            public void ShouldThrowRecordNotModifiedException_WhenStoreModelIsValidAndNoMatchIsPresent()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(0);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                Func<Task> result = () => storeRepository.UpdateAsync(storeModel, default);

                // Aseert
                result.Should().Throw<RecordNotModifiedException>();

                databaseIntegratorMock.Verify(integrator => integrator.ReplaceAsync(id, It.IsAny<StoreEntity>(), default), Times.Once);
            }

            [Fact]
            public async Task ShouldNotSetResponseRevisionInRevisionContext_WhenStoreModelIsValidAndNoMatchIsPresent()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(0);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                try
                {
                    await storeRepository.UpdateAsync(storeModel, default);
                }
                catch { }

                // Aseert
                revisionContext.ResponseRevision.Should().BeNull();
            }

            [Fact]
            public async Task ShouldNotSetResponseRevisionInRevisionContext_WhenStoreModelIsValidWithRequestRevisionAndNoMatchIsPresent()
            {
                // Arrange
                var id = "/test";
                var json = "{ \"myValue\": \"test\" }";
                var payload = JsonDocument.Parse(json).RootElement;

                var storeModel = new StoreModel(id, payload);

                var revisionContext = new RevisionContext
                {
                    RequestRevision = Guid.NewGuid()
                };
                var databaseIntegratorMock = GetDataIntegratorMock(0);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                try
                {
                    await storeRepository.UpdateAsync(storeModel, default);
                }
                catch { }

                // Aseert
                revisionContext.ResponseRevision.Should().BeNull();
            }

            private Mock<IDatabaseIntegrator<StoreEntity>> GetDataIntegratorMock(long modificationResult)
            {
                var databaseModificationResult = Task.FromResult(new DataModificationResult(modificationResult));

                var databaseIntegratorMock = new Mock<IDatabaseIntegrator<StoreEntity>>();
                databaseIntegratorMock.Setup(integrator => integrator.ReplaceAsync(It.IsAny<string>(), It.IsAny<StoreEntity>(), default))
                    .Returns(databaseModificationResult)
                    .Callback<string, StoreEntity, CancellationToken>((id, entity, cancellationToken) =>
                    {
                        _storeEntity = entity;
                    });

                databaseIntegratorMock.Setup(integrator => integrator.ReplaceAsync(It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<StoreEntity>(), default))
                    .Returns(databaseModificationResult)
                    .Callback<string, Guid?, StoreEntity, CancellationToken>((id, revision, entity, cancellationToken) =>
                    {
                        _storeEntity = entity;
                    });

                return databaseIntegratorMock;
            }
        }

        public class TheDeleteAsyncMethod
        {
            [Fact]
            public async Task ShouldCallDeleteAsync_WhenGivenAnIdAndExistingStoreModelWithNoRequestRevision()
            {
                // Arrange
                var id = "/test";

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(1);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                await storeRepository.DeleteAsync(id, default);

                // Aseert
                databaseIntegratorMock.Verify(integrator => integrator.DeleteAsync(id, default), Times.Once);
            }

            [Fact]
            public async Task ShouldCallDeleteAsyncWithRequestRevision_WhenGivenAnIdAndExistingStoreModelWithRequestRevision()
            {
                // Arrange
                var id = "/test";

                var revisionContext = new RevisionContext
                {
                    RequestRevision = Guid.NewGuid()
                };
                var databaseIntegratorMock = GetDataIntegratorMock(1);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                await storeRepository.DeleteAsync(id, default);

                // Aseert
                databaseIntegratorMock.Verify(integrator => integrator.DeleteAsync(id, revisionContext.RequestRevision, default), Times.Once);
            }

            [Fact]
            public void ShouldThrowRecordNotModifiedException_WhenGivenAnIdAndExistingStoreModelWithRequestRevisionAndNoMatchIsPresent()
            {
                // Arrange
                var id = "/test";

                var revisionContext = new RevisionContext
                {
                    RequestRevision = Guid.NewGuid()
                };
                var databaseIntegratorMock = GetDataIntegratorMock(0);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                Func<Task> result = () => storeRepository.DeleteAsync(id, default);

                // Aseert
                result.Should().Throw<RecordNotModifiedException>();

                databaseIntegratorMock.Verify(integrator => integrator.DeleteAsync(id, revisionContext.RequestRevision, default), Times.Once);
            }

            [Fact]
            public void ShouldThrowRecordNotModifiedException_WhenGivenAnIdAndNoMatchIsPresent()
            {
                // Arrange
                var id = "/test";

                var revisionContext = new RevisionContext();
                var databaseIntegratorMock = GetDataIntegratorMock(0);

                var storeRepository = new StoreRepository(databaseIntegratorMock.Object, revisionContext);

                // Act
                Func<Task> result = () => storeRepository.DeleteAsync(id, default);

                // Aseert
                result.Should().Throw<RecordNotModifiedException>();

                databaseIntegratorMock.Verify(integrator => integrator.DeleteAsync(id, default), Times.Once);
            }

            private Mock<IDatabaseIntegrator<StoreEntity>> GetDataIntegratorMock(long modificationResult)
            {
                var databaseModificationResult = Task.FromResult(new DataModificationResult(modificationResult));

                var databaseIntegratorMock = new Mock<IDatabaseIntegrator<StoreEntity>>();
                databaseIntegratorMock.Setup(integrator => integrator.DeleteAsync(It.IsAny<string>(), default))
                    .Returns(databaseModificationResult);

                databaseIntegratorMock.Setup(integrator => integrator.DeleteAsync(It.IsAny<string>(), It.IsAny<Guid?>(), default))
                    .Returns(databaseModificationResult);

                return databaseIntegratorMock;
            }
        }

        private class RevisionContext : IRevisionContext
        {
            public Guid? RequestRevision { get; set; }
            public Guid? ResponseRevision { get; set; }
        }
    }
}
