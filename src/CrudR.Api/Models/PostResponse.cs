namespace CrudR.Api.Models
{
#pragma warning disable CA1054 // Uri parameters should not be strings
#pragma warning disable CA1056 // Uri properties should not be strings
    /// <summary>
    /// Response for a POST request
    /// </summary>
    public class PostResponse
    {
        /// <summary>
        /// The comstructor
        /// </summary>
        /// <param name="uri"></param>
        public PostResponse(string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// Uri fragment that the resource was generated at.
        /// </summary>
        public string Uri { get; }
    }
#pragma warning restore CA1056 // Uri properties should not be strings
#pragma warning restore CA1054 // Uri parameters should not be strings
}
