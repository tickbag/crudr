using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CrudR.Api.Models;
using CrudR.Core.Models;
using CrudR.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrudR.Api.Controllers
{
    /// <summary>
    /// The Store Api Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType((int)HttpStatusCode.PreconditionRequired, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ProblemDetails))]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        /// <summary>
        /// The controller constructor
        /// </summary>
        /// <param name="storeService">The Store Service instance</param>
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        /// <summary>
        /// Get a stored json object from a particular uri fragment
        /// </summary>
        /// <param name="uri">The uri fragment of the stored json object</param>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns>A stored json object</returns>
        [HttpGet("{**uri}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(object))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ProblemDetails))]
        public async Task<object> GetAsync([Required] string uri, CancellationToken cancellationToken) =>
            (await _storeService.ReadStoreAsync(uri, cancellationToken))?.Payload;

        /// <summary>
        /// Create a store definition with or without data at a particular uri fragment
        /// </summary>
        /// <param name="uri">The uri fragment to store at</param>
        /// <param name="json">A json object to store</param>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns>204 (Created) if successful</returns>
        [HttpPost("{**uri}")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(PostResponse))]
        [ProducesResponseType((int)HttpStatusCode.Forbidden, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> PostAsync([Required] string uri, [FromBody] JsonElement json, CancellationToken cancellationToken)
        {
            var storeModel = new StoreModel(uri, json);

            await _storeService.CreateStoreAsync(storeModel, cancellationToken);

            return CreatedAtAction("Post", new { uri }, new PostResponse(uri));
        }

        /// <summary>
        /// Update the stored data at a particular uri fragment.
        /// The updated json data must be of the same structure as the original json stored there, only the values can be changed.
        /// </summary>
        /// <param name="uri">The uri fragment</param>
        /// <param name="json">A json object of the new data</param>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns>Ok (200) if successful</returns>
        [HttpPut("{**uri}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(object))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> PutAsync([Required] string uri, [FromBody] JsonElement json, CancellationToken cancellationToken)
        {
            var storeModel = new StoreModel(uri, json);

            await _storeService.UpdateStoreAsync(storeModel, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Delete the stored data at a particular uri fragment
        /// </summary>
        /// <param name="uri">The uri fragment</param>
        /// <param name="cancellationToken">Cancellation token for the request</param>
        /// <returns>Ok (200) if successful</returns>
        [HttpDelete("{**uri}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(object))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(ProblemDetails))]
        public async Task DeleteAsync([Required] string uri, CancellationToken cancellationToken) =>
            await _storeService.DeleteStoreAsync(uri, cancellationToken);
    }
}
