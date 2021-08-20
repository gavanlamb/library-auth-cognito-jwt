using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Expensely.Authentication.Cognito.Jwt.Extensions
{
    public static class ServiceCollection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services">Service collection to add authentication to</param>
        /// <param name="configuration">Configuration provider</param>
        /// <param name="configurationName">Name of the element that contains the relevant configuration</param>
        /// <returns></returns>
        public static void AddCognitoJwt(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationName = "Auth")
        {
            
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var config = configuration.GetSection(configurationName).Get<Options.Auth>();

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                        {
                            var json = new WebClient().DownloadString(config.JwtKeySetUrl);
            
                            var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
            
                            return keys;
                        },
                        ValidIssuer = config.Issuer,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidAudiences = config.Audiences
                    };
                });
        }
    }
}