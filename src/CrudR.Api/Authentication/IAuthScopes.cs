namespace CrudR.Api.Authentication
{
    /// <summary>
    /// Define the Authorisation scopes to use for each of the Http methods
    /// </summary>
    public interface IAuthScopes
    {
        /// <summary>
        /// Sets the authorisation scope for the GET Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        string GetScope { get; }

        /// <summary>
        /// Sets the authorsation scope for the POST Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        string PostScope { get; }

        /// <summary>
        /// Sets the authorsation scope for the PUT Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        string PutScope { get; }

        /// <summary>
        /// Sets the authorsation scope for the DELETE Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        string DeleteScope { get; }
    }
}
