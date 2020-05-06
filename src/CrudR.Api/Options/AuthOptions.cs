using CrudR.Api.Authentication;

namespace CrudR.Api.Options
{
    /// <summary>
    /// Defines the Authentication options for CrudR
    /// </summary>
    internal class AuthOptions : IAuthOptions
    {
        /// <summary>
        /// The OAuth Audience for the CrudR Api
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// The Authority or Issuer of the bearer token. Usually this is just the Uri of the Auth server.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Set this to true if you want CrudR to use a locally provided signing key for validating the JWT signature.
        /// If this is false, CrudR will attempt to automatically discover the signing key from the Authority address.
        /// </summary>
        public bool UseLocalIssuerSigningKey { get; set; }

        /// <summary>
        /// The local signing key to use for validation, in string format.
        /// Alternatively you can set the IssuerSigningKeyFilePath instead if that suits you architecture better.
        /// </summary>
        public string IssuerSigningKey { get; set; }

        /// <summary>
        /// The file path to the local X509 signing key for validation.
        /// Alternatively you can set the IssuerSigningKey instead, if you have the token as a string/secret.
        /// </summary>
        public string IssuerSigningKeyFilePath { get; set; }
    }
}
