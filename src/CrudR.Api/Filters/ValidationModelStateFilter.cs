using System.Linq;
using System.Threading.Tasks;
using CrudR.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CrudR.Api.Filters
{
    /// <summary>
    /// Validate the model state and return <see cref="ErrorResponse"/> if there are errors
    /// </summary>
    internal class ValidateModelStateFilter : IAsyncActionFilter
    {
        private const string ModelStateErrorCode = "ValidationError";
        private const string NewLine = "\n";
        private readonly ILogger _logger;

        /// <summary>
        /// The Validate Model State Filter constructor
        /// </summary>
        /// <param name="logger">An ILogger interface</param>
        public ValidateModelStateFilter(ILogger<ValidateModelStateFilter> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(err => err.Value.Errors.Any())
                    .Select(err => $"{ModelStateErrorCode} {err.Key}: {err.Value.Errors.Select(ev => ev.ErrorMessage).FirstOrDefault()}")
                    .ToArray();
                context.Result = new BadRequestObjectResult(new ErrorResponse(string.Join(NewLine, errors)));

                _logger.LogDebug(ModelStateErrorCode, context.Result);

                return Task.CompletedTask;
            }

            return next();
        }
    }
}
