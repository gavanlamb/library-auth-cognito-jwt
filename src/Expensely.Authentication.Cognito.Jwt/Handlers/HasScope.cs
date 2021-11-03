using System.Linq;
using System.Threading.Tasks;
using Expensely.Authentication.Cognito.Jwt.Clients;
using Microsoft.AspNetCore.Authorization;
using HasScopeRequirement = Expensely.Authentication.Cognito.Jwt.Requirements.HasScope;

namespace Expensely.Authentication.Cognito.Jwt.Handlers
{
    public class HasScope: AuthorizationHandler<HasScopeRequirement>
    {
        private readonly IUserPoolClient _client;
        public HasScope(
            IUserPoolClient client)
        {
            _client = client;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "client_id" && c.Issuer == requirement.Issuer))
                return;

            var clientId = context.User.Claims.FirstOrDefault(x => x.Type == "client_id");
            if (clientId == null)
                return;
            
            var scopes = await _client.GetOAuthScopes(clientId.Value);
            
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return;

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);
        }
    }
}