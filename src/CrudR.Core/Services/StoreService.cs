using System;
using System.Threading;
using System.Threading.Tasks;
using CrudR.Core.Models;
using CrudR.Core.Repositories;
using CrudR.Core.Validators;

namespace CrudR.Core.Services
{
    /// <summary>
    /// Store service containing main business logic
    /// </summary>
    internal class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IStoreModelValidator _storeModelValidator;

        /// <summary>
        /// The Store Service constructor
        /// </summary>
        /// <param name="storeRepository">The Store Repository instance</param>
        /// <param name="storeModelValidator">The Store Validation service instance</param>
        public StoreService(IStoreRepository storeRepository, IStoreModelValidator storeModelValidator)
        {
            _storeRepository = storeRepository;
            _storeModelValidator = storeModelValidator;
        }

        /// <summary>
        /// Create a new entry in the store
        /// </summary>
        /// <param name="storeModel">The new store data model</param>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns>An awaitable Task</returns>
        public async Task CreateStoreAsync(StoreModel storeModel, CancellationToken cancellationToken) =>
            await _storeRepository.CreateAsync(storeModel, cancellationToken);

        /// <summary>
        /// Read an entry from the store with this id
        /// </summary>
        /// <param name="id">The Id of the data to read</param>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns>The stored data model for that Id</returns>
        public async Task<StoreModel> ReadStoreAsync(string id, CancellationToken cancellationToken) =>
            await _storeRepository.ReadAsync(id, cancellationToken);

        /// <summary>
        /// Update an existing entry in the store.
        /// The data definition of the payload must match what is currently stored.
        /// </summary>
        /// <param name="storeModel">The updated store data model</param>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns>An awaitable Task</returns>
        public async Task UpdateStoreAsync(StoreModel storeModel, CancellationToken cancellationToken)
        {
            _ = storeModel ?? throw new ArgumentNullException(nameof(storeModel));

            var stored = await _storeRepository.ReadAsync(storeModel.Id, cancellationToken);

            _storeModelValidator.Validate(storeModel, stored);

            await _storeRepository.UpdateAsync(storeModel, cancellationToken);
        }

        /// <summary>
        /// Delete an entry in the store with this id
        /// </summary>
        /// <param name="id">The Id of the data to delete</param>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns>An awaitable Task</returns>
        public async Task DeleteStoreAsync(string id, CancellationToken cancellationToken) =>
            await _storeRepository.DeleteAsync(id, cancellationToken);
    }
}
