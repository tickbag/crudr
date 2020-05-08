using System.Security.Cryptography.X509Certificates;
using System.Text;
using CrudR.Api.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CrudR.Api.Authentication
{
    /// <summary>
    /// Extension methods for configuring Authentication and Authorisation within the CrudR application
    /// </summary>
    internal static class AuthConfigurationExtensions
    {
        /// <summary>
        /// Add Authentication and Authorisation to the available app services
        /// </summary>
        /// <param name="services">The services collection to be configured</param>
        /// <param name="authOptions">An AuthOptions instance containing Auth configuration data</param>
        /// <param name="authClaims">An AuthClaims instance containing Claims configuration data</param>
        public static void AddCrudRAuthentication(this IServiceCollection services,
            IAuthOptions authOptions,
            IAuthClaims authClaims)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Audience = authOptions.Audience;

                    if (!authOptions.UseLocalIssuerSigningKey)
                        options.Authority = authOptions.Authority;

                    if (authOptions.UseLocalIssuerSigningKey)
                    {
                        var signingKey = string.IsNullOrEmpty(authOptions.IssuerSigningKey) ?
                            new X509SecurityKey(new X509Certificate2(authOptions.IssuerSigningKeyFilePath)) :
                            new X509SecurityKey(new X509Certificate2(Encoding.ASCII.GetBytes(authOptions.IssuerSigningKey)));

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidIssuer = authOptions.Authority,
                            IssuerSigningKey = signingKey
                        };
                    }
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Get", policy =>
                    policy.AddRequirements(DiscernRequirements(authClaims.GetAllowAnonymous, authClaims.GetClaim, authClaims.GetClaimValue)));

                options.AddPolicy("Post", policy =>
                    policy.AddRequirements(DiscernRequirements(authClaims.PostAllowAnonymous, authClaims.PostClaim, authClaims.PostClaimValue)));

                options.AddPolicy("Put", policy =>
                    policy.AddRequirements(DiscernRequirements(authClaims.PutAllowAnonymous, authClaims.PutClaim, authClaims.PutClaimValue)));

                options.AddPolicy("Delete", policy =>
                    policy.AddRequirements(DiscernRequirements(authClaims.DeleteAllowAnonymous, authClaims.DeleteClaim, authClaims.DeleteClaimValue)));
            });
        }

        /// <summary>
        /// Adds the Security Descriptor for the OpenAPI/Swagger document
        /// </summary>
        /// <param name="swagger">The SwaggerGen options instance to configure</param>
        public static void AddOpenApiSecurityDefinition(this SwaggerGenOptions swagger)
        {
            swagger.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Bearer Token Authentication header",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            swagger.OperationFilter<AuthenticationRequirementsOperationFilter>();
        }

        private static IAuthorizationRequirement DiscernRequirements(bool allowAnonymous, string claim, string claimValue)
        {
            if (allowAnonymous)
                return new AssertionRequirement((context) => true);

            if (string.IsNullOrEmpty(claim))
                return new DenyAnonymousAuthorizationRequirement();

            return new ClaimsAuthorizationRequirement(claim, string.IsNullOrEmpty(claimValue) ? null : new[] { claimValue });
        }
    }
}
