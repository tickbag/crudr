using CrudR.Api.Authentication;

namespace CrudR.Api.Options
{
    /// <summary>
    /// Define the Authorisation scopes to use for each of the Http methods
    /// </summary>
    internal class AuthScopes : IAuthScopes
    {
        /// <summary>
        /// Sets the authorisation scope for the GET Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        public string GetScope { get; set; }

        /// <summary>
        /// Sets the authorsation scope for the POST Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        public string PostScope { get; set; }

        /// <summary>
        /// Sets the authorsation scope for the PUT Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        public string PutScope { get; set; }

        /// <summary>
        /// Sets the authorsation scope for the DELETE Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        public string DeleteScope { get; set; }
    }
}
