using CrudR.Api.Authentication;

namespace CrudR.Api.Options
{
    /// <summary>
    /// Define the Authorisation scopes to use for each of the Http methods
    /// </summary>
    internal class AuthClaims : IAuthClaims
    {
        /// <summary>
        /// Allow anonymoud access to the GET method.
        /// If this is set true, no authentication is required to access the method and the related Claim and ClaimValue settings are ignored.
        /// </summary>
        public bool GetAllowAnonymous { get; set; }

        /// <summary>
        /// Sets the authorisation claim for the GET Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        public string GetClaim { get; set; }

        /// <summary>
        /// Sets the value to match in the authorization claim for the GET method.
        /// This value will match against an array of values or a simple string.
        /// If this value is empty, then claim matching will be done on the Claim type only.
        /// </summary>
        public string GetClaimValue { get; set; }

        /// <summary>
        /// Allow anonymoud access to the POST method.
        /// If this is set true, no authentication is required to access the method and the related Claim and ClaimValue settings are ignored.
        /// </summary>
        public bool PostAllowAnonymous { get; set; }

        /// <summary>
        /// Sets the authorsation claim for the POST Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        public string PostClaim { get; set; }

        /// <summary>
        /// Sets the value to match in the authorization claim for the POST method.
        /// This value will match against an array of values or a simple string.
        /// If this value is empty, then claim matching will be done on the Claim type only.
        /// </summary>
        public string PostClaimValue { get; set; }

        /// <summary>
        /// Allow anonymoud access to the PUT method.
        /// If this is set true, no authentication is required to access the method and the related Claim and ClaimValue settings are ignored.
        /// </summary>
        public bool PutAllowAnonymous { get; set;  }

        /// <summary>
        /// Sets the authorsation claim for the PUT Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        public string PutClaim { get; set; }

        /// <summary>
        /// Sets the value to match in the authorization claim for the PUT method.
        /// This value will match against an array of values or a simple string.
        /// If this value is empty, then claim matching will be done on the Claim type only.
        /// </summary>
        public string PutClaimValue { get; set; }

        /// <summary>
        /// Allow anonymoud access to the DELETE method.
        /// If this is set true, no authentication is required to access the method and the related Claim and ClaimValue settings are ignored.
        /// </summary>
        public bool DeleteAllowAnonymous { get; set; }

        /// <summary>
        /// Sets the authorsation claim for the DELETE Method.
        /// Null or empty will allow anyone to access this method.
        /// </summary>
        public string DeleteClaim { get; set; }

        /// <summary>
        /// Sets the value to match in the authorization claim for the DELETE method.
        /// This value will match against an array of values or a simple string.
        /// If this value is empty, then claim matching will be done on the Claim type only.
        /// </summary>
        public string DeleteClaimValue { get; set; }
    }
}
