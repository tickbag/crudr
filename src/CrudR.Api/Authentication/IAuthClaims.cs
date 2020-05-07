namespace CrudR.Api.Authentication
{
    /// <summary>
    /// Define the Authorisation scopes to use for each of the Http methods
    /// </summary>
    public interface IAuthClaims
    {
        /// <summary>
        /// Allow anonymoud access to the GET method.
        /// If this is set true, no authentication is required to access the method and the related Claim and ClaimValue settings are ignored.
        /// </summary>
        bool GetAllowAnonymous { get; }

        /// <summary>
        /// Sets the authorisation claim for the GET Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        string GetClaim { get; }

        /// <summary>
        /// Sets the value to match in the authorization claim for the GET method.
        /// This value will match against an array of values or a simple string.
        /// If this value is empty, then claim matching will be done on the Claim type only.
        /// </summary>
        string GetClaimValue { get; }

        /// <summary>
        /// Allow anonymoud access to the POST method.
        /// If this is set true, no authentication is required to access the method and the related Claim and ClaimValue settings are ignored.
        /// </summary>
        bool PostAllowAnonymous { get; }

        /// <summary>
        /// Sets the authorsation claim for the POST Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        string PostClaim { get; }

        /// <summary>
        /// Sets the value to match in the authorization claim for the POST method.
        /// This value will match against an array of values or a simple string.
        /// If this value is empty, then claim matching will be done on the Claim type only.
        /// </summary>
        string PostClaimValue { get; }

        /// <summary>
        /// Allow anonymoud access to the PUT method.
        /// If this is set true, no authentication is required to access the method and the related Claim and ClaimValue settings are ignored.
        /// </summary>
        bool PutAllowAnonymous { get; }

        /// <summary>
        /// Sets the authorsation claim for the PUT Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        string PutClaim { get; }

        /// <summary>
        /// Sets the value to match in the authorization claim for the PUT method.
        /// This value will match against an array of values or a simple string.
        /// If this value is empty, then claim matching will be done on the Claim type only.
        /// </summary>
        string PutClaimValue { get; }

        /// <summary>
        /// Allow anonymoud access to the DELETE method.
        /// If this is set true, no authentication is required to access the method and the related Claim and ClaimValue settings are ignored.
        /// </summary>
        bool DeleteAllowAnonymous { get; }

        /// <summary>
        /// Sets the authorsation claim for the DELETE Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        string DeleteClaim { get; }

        /// <summary>
        /// Sets the value to match in the authorization claim for the DELETE method.
        /// This value will match against an array of values or a simple string.
        /// If this value is empty, then claim matching will be done on the Claim type only.
        /// </summary>
        string DeleteClaimValue { get; }
    }
}
