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

        /// <summary>
        /// The base uri that data will be stored at.
        /// This should not start or end with a slash, but my have slashes within the string. i.e. 'api/v1'
        /// Defaults to empty
        /// </summary>
        string BaseUri { get; }

        /// <summary>
        /// Set to true to turn on Api Authentication.
        /// </summary>
        bool UseAuthentication { get; }

        /// <summary>
        /// Enable Healthcheck endpoint on the CrudR API
        /// </summary>
        bool UseHealthChecks { get; }

        /// <summary>
        /// If UseHealthChecks is set to true, this property set the endpoint Uri for the liveness probe to hit.
        /// </summary>
        string LivenessEndpoint { get; }

        /// <summary>
        /// If UseHealthChecks is set to true, this property set the endpoint Uri for the readiness probe to hit.
        /// </summary>
        string ReadinessEndpoint { get; }
    }
}
