using System.ComponentModel.DataAnnotations;

namespace CrudR.Api.Authentication
{
    /// <summary>
    /// Defines the Authentication options for CrudR
    /// </summary>
    public interface IAuthOptions
    {
        /// <summary>
        /// The OAuth Audience for the CrudR Api
        /// </summary>
        [Required]
        string Audience { get; }

        /// <summary>
        /// The Authority or Issuer of the bearer token. Usually this is just the Uri of the Auth server.
        /// </summary>
        [Required]
        string Authority { get; }

        /// <summary>
        /// Set this to true if you want CrudR to use a locally provided signing key for validating the JWT signature.
        /// If this is false, CrudR will attempt to automatically discover the signing key from the Authority address.
        /// </summary>
        bool UseLocalIssuerSigningKey { get; }

        /// <summary>
        /// The local signing key to use for validation, in string format.
        /// Alternatively you can set the IssuerSigningKeyFilePath instead if that suits you architecture better.
        /// </summary>
        string IssuerSigningKey { get; }

        /// <summary>
        /// The file path to the local X509 signing key for validation.
        /// Alternatively you can set the IssuerSigningKey instead, if you have the token as a string/secret.
        /// </summary>
        string IssuerSigningKeyFilePath { get; }
    }
}
