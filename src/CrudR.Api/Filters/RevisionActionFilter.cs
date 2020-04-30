using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudR.Api.Exceptions;
using CrudR.Api.Options;
using CrudR.Context.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudR.Api.Filters
{
    /// <summary>
    /// Action filter to convert data revision information to and from a request context
    /// </summary>
    internal class RevisionActionFilter : IAsyncActionFilter
    {
        private const string RevisionRequestHeaderName = "If-Match";
        private const string RevisionResponseHeaderName = "ETag";

        private readonly List<string> _excludeMethodsFromRevisionRequirement = new List<string>
        {
            "GET",
            "POST"
        };

        private readonly IRevisionContext _revisionContext;
        private readonly bool _requireRevisionMatching;

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="revisionContext">A Request Scoped Revision context</param>
        /// <param name="appOptions">Application options</param>
        public RevisionActionFilter(IRevisionContext revisionContext, IApplicationOptions appOptions)
        {
            _revisionContext = revisionContext;
            _requireRevisionMatching = appOptions.RequireRevisionMatching;
        }

        /// <inheritDoc />
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ifMatch = context.HttpContext.Request.Headers[RevisionRequestHeaderName];

            if (Guid.TryParse(ifMatch, out var revision))
                _revisionContext.RequestRevision = revision;

            if (_requireRevisionMatching && !_revisionContext.RequestRevision.HasValue)
            {
                if (!_excludeMethodsFromRevisionRequirement.Contains(context.HttpContext.Request.Method.ToUpperInvariant()))
                    throw new RequiredPreconditionInvalidException();
            }

            await next();

            if (_revisionContext.ResponseRevision.HasValue)
                context.HttpContext.Response.Headers.Add(RevisionResponseHeaderName, _revisionContext.ResponseRevision.ToString());
        }
    }
}
