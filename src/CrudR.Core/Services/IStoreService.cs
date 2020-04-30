using System.Threading;
using System.Threading.Tasks;
using CrudR.Core.Models;

namespace CrudR.Core.Services
{
    /// <summary>
    /// The Store service handling any business logic for the Store model
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// Create a Json payload store
        /// </summary>
        /// <param name="storeModel">The Json payload model</param>
        /// <param name="cancellationToken">A Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task CreateStoreAsync(StoreModel storeModel, CancellationToken cancellationToken);

        /// <summary>
        /// Delete an rxisting Json payload store
        /// </summary>
        /// <param name="id">The Id of the store</param>
        /// <param name="cancellationToken">A Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task DeleteStoreAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Read the contents of a Json store
        /// </summary>
        /// <param name="id">The Id of the store</param>
        /// <param name="cancellationToken">A Cancellayion token</param>
        /// <returns>The stored Json payload model</returns>
        Task<StoreModel> ReadStoreAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Update the contents of a Json store.
        /// The update payload must match the schema of the Json currently stored and being updated
        /// </summary>
        /// <param name="storeModel">The Json payload model</param>
        /// <param name="cancellationToken">A Cancellation token</param>
        /// <returns>An awaitable task</returns>
        Task UpdateStoreAsync(StoreModel storeModel, CancellationToken cancellationToken);
    }
}
