using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using Expensely.Authentication.Cognito.Jwt.Clients;
using Expensely.Authentication.Cognito.Jwt.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using HasScopeRequirement = Expensely.Authentication.Cognito.Jwt.Requirements.HasScope;
using HasScopeHandler = Expensely.Authentication.Cognito.Jwt.Handlers.HasScope;

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
        public static IServiceCollection AddCognitoJwt(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationName = "Auth")
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var config = configuration.GetSection(configurationName).Get<Auth>();

            if (string.IsNullOrWhiteSpace(config.Issuer))
                throw new ArgumentException($"{configurationName}:Issuer is null or empty");
            
            if (string.IsNullOrWhiteSpace(config.UserPoolId))
                throw new ArgumentException($"{configurationName}:UserPoolId is null or empty");
            
            if (string.IsNullOrWhiteSpace(config.JwtKeySetUrl))
                throw new ArgumentException($"{configurationName}:JwtKeySetUrl is null or empty");
            
            if (config.Scopes == null || !config.Scopes.Any())
                throw new ArgumentException($"{configurationName}:Scopes is null or empty");
            
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                        {
                            var json = new WebClient().DownloadString(config.JwtKeySetUrl);

                            return JsonWebKeySet.Create(json).Keys;
                        },
                        ValidIssuer = config.Issuer,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = false
                    };
                });

            services
                .AddAuthorization(options =>
                {
                    foreach (var scopeConfig in config.Scopes)
                    {
                        options.AddPolicy(
                            scopeConfig.Key,
                            policy =>
                            {
                                foreach (var scope in scopeConfig.Value)
                                {
                                    policy.Requirements.Add(new HasScopeRequirement(scope, config.Issuer));
                                }
                            });
                    }
                });
            
            services.Configure<Auth>(configuration);
            
            services.TryAddScoped<IAuthorizationHandler, HasScopeHandler>();
            services.TryAddScoped<IUserPoolClient, UserPoolClient>();

            return services;
        }
    }
}