using System;

namespace CrudR.Context.Abstractions
{
    /// <summary>
    /// Data concurrency revision information
    /// </summary>
    public interface IRevisionContext
    {
        /// <summary>
        /// Unique Id indicating the data revision being used in this request
        /// </summary>
        Guid? RequestRevision { get; set; }

        /// <summary>
        /// Unique Id indicating the revision of the data being returned by this request
        /// </summary>
        Guid? ResponseRevision { get; set; }
    }
}
