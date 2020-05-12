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

        /// <summary>
        /// Enable Healthcheck endpoint on the CrudR API
        /// </summary>
        public bool UseHealthChecks { get; set; }

        /// <summary>
        /// If UseHealthChecks is set to true, this property set the endpoint Uri for the liveness probe to hit.
        /// If this is left blank, no readiness check endpoint will be registered.
        /// </summary>
        public string LivenessEndpoint { get; set; }

        /// <summary>
        /// If UseHealthChecks is set to true, this property set the endpoint Uri for the readiness probe to hit.
        /// If this is left blank, no readiness check endpoint will be registered.
        /// </summary>
        public string ReadinessEndpoint { get; set; }
    }
}
