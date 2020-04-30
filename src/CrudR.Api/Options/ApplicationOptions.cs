namespace CrudR.Api.Options
{
    /// <summary>
    /// Configuration options for the application
    /// </summary>
    internal class ApplicationOptions : IApplicationOptions
    {
        /// <summary>
        /// Enable the Swagger UI. Defaults to false.
        /// </summary>
        public bool EnableSwaggerUI { get; set; }

        /// <summary>
        /// Require requests to provide a data revision 'If-Match' header
        /// </summary>
        public bool RequireRevisionMatching { get; set; }
    }
}
