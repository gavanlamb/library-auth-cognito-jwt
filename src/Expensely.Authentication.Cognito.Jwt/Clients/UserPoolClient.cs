using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Expensely.Authentication.Cognito.Jwt.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Expensely.Authentication.Cognito.Jwt.Clients
{
    public class UserPoolClient : IUserPoolClient
    {
        private readonly IAmazonCognitoIdentityProvider _client;
        private readonly Auth _authOptions;
        private readonly ILogger<UserPoolClient> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _defaultCacheEntryOptions;
        public UserPoolClient(
            ILogger<UserPoolClient> logger,
            IOptions<Auth> authOptions,
            IAmazonCognitoIdentityProvider client,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _authOptions = authOptions.Value;
            _client = client;
            _memoryCache = memoryCache;
            _defaultCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        }

        public async Task<IEnumerable<string>> GetOAuthScopes(
            string clientId)
        {
            try
            {
                var request = new DescribeUserPoolClientRequest
                {
                    ClientId = clientId,
                    UserPoolId = _authOptions.UserPoolId
                };
                
                var key = $"{_authOptions.UserPoolId}:{clientId}";
                if (_memoryCache.TryGetValue(key, out IEnumerable<string> cacheValue))
                {
                    return cacheValue;
                }
                var response = await _client.DescribeUserPoolClientAsync(request);
                if (response.HttpStatusCode >= HttpStatusCode.BadRequest)
                {
                    return Enumerable.Empty<string>();
                }

                response.UserPoolClient.AllowedOAuthScopes.Remove("email");
                response.UserPoolClient.AllowedOAuthScopes.Remove("phone");
                response.UserPoolClient.AllowedOAuthScopes.Remove("openid");
                response.UserPoolClient.AllowedOAuthScopes.Remove("profile");

                return _memoryCache.Set(
                    key, 
                    response.UserPoolClient.AllowedOAuthScopes, 
                    _defaultCacheEntryOptions);
            }
            catch (InternalErrorException exception)
            {
                _logger.LogError(
                    exception,
                    "Internal error encountered");
                throw;
            }
            catch (InvalidParameterException exception)
            {
                _logger.LogError(
                    exception,
                    "Invalid parameter");
                throw;
            }
            catch (NotAuthorizedException exception)
            {
                _logger.LogError(
                    exception,
                    "Not authorised to access userpool client");
                throw;
            }
            catch (ResourceNotFoundException exception)
            {
                _logger.LogError(
                    exception,
                    "Userpool client not found");
                throw;
            }
            catch (TooManyRequestsException exception)
            {
                _logger.LogError(
                    exception,
                    "Too many requests to cognito");
                throw;
            }
        }
    }
}