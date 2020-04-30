using System.Threading;
using System.Threading.Tasks;
using CrudR.Core.Models;

namespace CrudR.Core.Repositories
{
    /// <summary>
    /// Interface defining the data access and storage contract for the Store Model
    /// </summary>
    public interface IStoreRepository
    {
        /// <summary>
        /// Create a new store entry.
        /// </summary>
        /// <param name="storeModel">Store model</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task CreateAsync(StoreModel storeModel, CancellationToken cancellationToken);

        /// <summary>
        /// Read the model stored at the provided Id.
        /// </summary>
        /// <param name="id">Id of the model</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The model stored with the Id provided</returns>
        Task<StoreModel> ReadAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Update an existing stored model
        /// </summary>
        /// <param name="storeModel">The new model that will replace the stored one. The Id's must match.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task UpdateAsync(StoreModel storeModel, CancellationToken cancellationToken);

        /// <summary>
        /// Delete an existing model.
        /// </summary>
        /// <param name="id">The Id of the model to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
