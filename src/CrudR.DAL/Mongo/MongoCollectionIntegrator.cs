using System;
using System.Threading;
using System.Threading.Tasks;
using CrudR.DAL.Entities;
using CrudR.DAL.Integration;
using CrudR.DAL.Options;
using MongoDB.Driver;

namespace CrudR.DAL.Mongo
{
    /// <summary>
    /// Integration handler for a MongoDb database
    /// </summary>
    /// <typeparam name="T">The type of data model to be read or stored in the Mongo Collection. Must implement IEntity</typeparam>
    internal class MongoCollectionIntegrator<T> : IDatabaseIntegrator<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoCollectionIntegrator(IDatabaseOptions settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// Insert a new entry into the Mongo Collection
        /// </summary>
        /// <param name="entity">Entry to insert include the Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries inserted</returns>
        public async Task<DataModificationResult> InsertAsync(T entity, CancellationToken cancellationToken)
        {
            long modified = 1;

            try
            {
                await _collection.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);
            }
            catch (MongoWriteException ex)
            {
                if (!ex.Message.Contains("duplicate key", StringComparison.InvariantCultureIgnoreCase))
                    throw;

                modified = 0;
            }

            return new DataModificationResult(modified);
        }

        /// <summary>
        /// Delete an entry from the Mongo Collection
        /// </summary>
        /// <param name="id">Unique Id of the entry</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries deleted</returns>
        public async Task<DataModificationResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _collection.DeleteOneAsync(BuildFilter(id), cancellationToken);
            return new DataModificationResult(result.DeletedCount);
        }

        /// <summary>
        /// Delete an entry from the Mongo Collection
        /// </summary>
        /// <param name="id">Unique Id of the entry</param>
        /// <param name="revision">The data revision number. If null delete the entry without matching the data revision.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries deleted</returns>
        public async Task<DataModificationResult> DeleteAsync(string id, Guid? revision, CancellationToken cancellationToken)
        {
            var result = await _collection.DeleteOneAsync(BuildFilterWithRevision(id, revision), cancellationToken);
            return new DataModificationResult(result.DeletedCount);
        }

        /// <summary>
        /// Find an entry in the Mongo Collection
        /// </summary>
        /// <param name="id">Unique Id of the entry</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The entry or null if not found</returns>
        public async Task<T> FindAsync(string id, CancellationToken cancellationToken)
        {
            var cursor = await _collection.FindAsync(BuildFilter(id), null, cancellationToken);
            return await cursor.SingleOrDefaultAsync();
        }

        /// <summary>
        /// Replace an existing entry in the Mongo Collection
        /// </summary>
        /// <param name="id">Unique Id of the existing entry</param>
        /// <param name="entity">New entry that will replace the existing oner</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries replaced</returns>
        public async Task<DataModificationResult> ReplaceAsync(string id, T entity, CancellationToken cancellationToken)
        {
            var result = await _collection.ReplaceOneAsync(BuildFilter(id), entity, (ReplaceOptions)null, cancellationToken);
            return new DataModificationResult(result.ModifiedCount);
        }

        /// <summary>
        /// Replace an existing entry in the Mongo Collection
        /// </summary>
        /// <param name="id">Unique Id of the existing entry</param>
        /// <param name="revision">Data revision of the existing entry</param>
        /// <param name="entity">New entry that will replace the existing oner</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries replaced</returns>
        public async Task<DataModificationResult> ReplaceAsync(string id, Guid? revision, T entity, CancellationToken cancellationToken)
        {
            var result = await _collection.ReplaceOneAsync(BuildFilterWithRevision(id, revision), entity, (ReplaceOptions)null, cancellationToken);
            return new DataModificationResult(result.ModifiedCount);
        }

        private FilterDefinition<T> BuildFilter(string id) =>
            Builders<T>.Filter.Eq(f => f.Id, id);

        private FilterDefinition<T> BuildFilterWithRevision(string id, Guid? revision)
        {
            var filterBuilder = Builders<T>.Filter;
            var filter = filterBuilder.Eq(f => f.Id, id) &
                filterBuilder.Eq(f => f.Revision, revision);

            return filter;
        }
    }
}
