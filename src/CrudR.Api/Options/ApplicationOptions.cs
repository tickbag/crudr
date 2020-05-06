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

        /// <summary>
        /// The base uri that data will be stored at.
        /// Defaults to empty
        /// </summary>
        public string BaseUri { get; set; } = "";

        /// <summary>
        /// Set to true to turn on Api Authentication.
        /// </summary>
        public bool UseAuthentication { get; set; }
    }
}
