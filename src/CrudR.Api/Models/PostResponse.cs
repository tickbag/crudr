namespace CrudR.Api.Models
{
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
}
