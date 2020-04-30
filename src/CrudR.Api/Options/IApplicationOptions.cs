namespace CrudR.Api.Options
{
    /// <summary>
    /// Configuration options for the application
    /// </summary>
    public interface IApplicationOptions
    {
        /// <summary>
        /// Enable the Swagger UI. Defaults to false.
        /// </summary>
        bool EnableSwaggerUI { get; }

        /// <summary>
        /// Require requests to provide a data revision 'If-Match' header
        /// </summary>
        bool RequireRevisionMatching { get; }
    }
}
