using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CrudR.Context.Abstractions;
using CrudR.Core.Exceptions;
using CrudR.Core.Models;
using CrudR.Core.Repositories;
using CrudR.DAL.Entities;
using CrudR.DAL.Integration;
using MongoDB.Bson;

namespace CrudR.DAL.Repositories
{
    /// <summary>
    /// Implementation of the <see cref="IStoreRepository"/> interface, handling data access and storage for the Storage Model
    /// </summary>
    internal class StoreRepository : IStoreRepository
    {
        private readonly IDatabaseIntegrator<StoreEntity> _databaseIntegrator;
        private readonly IRevisionContext _revisionContext;

        public StoreRepository(IDatabaseIntegrator<StoreEntity> databaseIntegrator,
            IRevisionContext revisionContext)
        {
            _databaseIntegrator = databaseIntegrator;
            _revisionContext = revisionContext;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(StoreModel storeModel, CancellationToken cancellationToken)
        {
            _ = storeModel ?? throw new ArgumentNullException(nameof(storeModel));

            var entity = new StoreEntity
            {
                Id = storeModel.Id,
                Revision = Guid.NewGuid(),
                Payload = BsonDocument.Parse(storeModel.Payload.GetRawText())
            };

            if ((await _databaseIntegrator.InsertAsync(entity, cancellationToken)).RecordsModified == 0)
                throw new RecordAlreadyExistsException();

            _revisionContext.ResponseRevision = entity.Revision;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(string id, CancellationToken cancellationToken) =>
            HandleResult(await DeleteEntityAsync(id, default));

        /// <inheritdoc/>
        public async Task<StoreModel> ReadAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _databaseIntegrator.FindAsync(id, cancellationToken) ??
                throw new RecordNotFoundException();

            _revisionContext.ResponseRevision = entity.Revision;

            return new StoreModel(entity.Id, JsonDocument.Parse(entity.Payload.ToJson()).RootElement);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(StoreModel storeModel, CancellationToken cancellationToken)
        {
            _ = storeModel ?? throw new ArgumentNullException(nameof(storeModel));

            var entity = new StoreEntity
            {
                Id = storeModel.Id,
                Revision = Guid.NewGuid(),
                Payload = BsonDocument.Parse(storeModel.Payload.GetRawText())
            };

            HandleResult(await UpdateEntityAsync(entity, cancellationToken));

            _revisionContext.ResponseRevision = entity.Revision;
        }

        private async Task<DataModificationResult> UpdateEntityAsync(StoreEntity entity, CancellationToken cancellationToken) =>
            _revisionContext.RequestRevision.HasValue ?
                await _databaseIntegrator.ReplaceAsync(entity.Id, _revisionContext.RequestRevision, entity, cancellationToken) :
                await _databaseIntegrator.ReplaceAsync(entity.Id, entity, cancellationToken);

        private async Task<DataModificationResult> DeleteEntityAsync(string id, CancellationToken cancellationToken) =>
            _revisionContext.RequestRevision.HasValue ?
                await _databaseIntegrator.DeleteAsync(id, _revisionContext.RequestRevision, cancellationToken) :
                await _databaseIntegrator.DeleteAsync(id, cancellationToken);

        private void HandleResult(DataModificationResult databaseResult)
        {
            if (databaseResult.RecordsModified == 0)
                throw new RecordNotModifiedException();
        }
    }
}
