using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Expensely.Authentication.Cognito.Jwt.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Expensely.Authentication.Cognito.Jwt.Clients
{
    public class UserPoolClient : IUserPoolClient
    {
        private readonly IAmazonCognitoIdentityProvider _client;
        private readonly Auth _authOptions;
        private readonly ILogger<UserPoolClient> _logger;
        public UserPoolClient(
            ILogger<UserPoolClient> logger,
            IOptions<Auth> authOptions,
            IAmazonCognitoIdentityProvider client)
        {
            _logger = logger;
            _authOptions = authOptions.Value;
            _client = client;
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
                
                //TODO ADD caching
                var response = await _client.DescribeUserPoolClientAsync(request);
                //TODO ADD caching

                return response.UserPoolClient.AllowedOAuthScopes;
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