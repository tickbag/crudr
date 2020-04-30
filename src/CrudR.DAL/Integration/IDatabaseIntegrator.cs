using System;
using System.Threading;
using System.Threading.Tasks;
using CrudR.DAL.Entities;

namespace CrudR.DAL.Integration
{
    /// <summary>
    /// Integration handler for the underlying database.
    /// Implement this interface to interact with a different database.
    /// </summary>
    /// <typeparam name="T">The type of data model to be read or stored in the database. Must implement IEntity</typeparam>
    public interface IDatabaseIntegrator<T> where T : IEntity
    {
        /// <summary>
        /// Delete an entry from the database
        /// </summary>
        /// <param name="id">Unique Id of the entry</param>
        /// <param name="revision">The data revision number. If null delete the entry without matching the data revision.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries deleted</returns>
        Task<DataModificationResult> DeleteAsync(string id, Guid? revision, CancellationToken cancellationToken);

        /// <summary>
        /// Delete an entry from the database
        /// </summary>
        /// <param name="id">Unique Id of the entry</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries deleted</returns>
        Task<DataModificationResult> DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Find an entry in the database
        /// </summary>
        /// <param name="id">Unique Id of the entry</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The entry or null if not found</returns>
        Task<T> FindAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Insert a new entry into the database
        /// </summary>
        /// <param name="entity">Entry to insert include the Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries inserted</returns>
        Task<DataModificationResult> InsertAsync(T entity, CancellationToken cancellationToken);

        /// <summary>
        /// Replace an existing entry in the database
        /// </summary>
        /// <param name="id">Unique Id of the existing entry</param>
        /// <param name="revision">Data revision of the existing entry</param>
        /// <param name="entity">New entry that will replace the existing oner</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries replaced</returns>
        Task<DataModificationResult> ReplaceAsync(string id, Guid? revision, T entity, CancellationToken cancellationToken);

        /// <summary>
        /// Replace an existing entry in the database
        /// </summary>
        /// <param name="id">Unique Id of the existing entry</param>
        /// <param name="entity">New entry that will replace the existing oner</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A <see cref="DataModificationResult"/> instance indicating the number of entries replaced</returns>
        Task<DataModificationResult> ReplaceAsync(string id, T entity, CancellationToken cancellationToken);
    }
}
