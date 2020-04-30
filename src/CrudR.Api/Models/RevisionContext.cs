using System;
using System.ComponentModel.DataAnnotations;
using CrudR.Context.Abstractions;

namespace CrudR.Api.Models
{
    /// <inheritdoc/>
    internal class RevisionContext : IRevisionContext
    {
        /// <inheritdoc/>
        [Display(Name = "If-Match", Description = "String representation of a Guid indicating the data revision to match for this request")]
        public Guid? RequestRevision { get; set; }

        /// <inheritdoc/>
        public Guid? ResponseRevision { get; set; }
    }
}
