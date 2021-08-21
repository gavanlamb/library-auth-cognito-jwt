using System.Linq;
using System.Threading.Tasks;
using Expensely.Authentication.Cognito.Jwt.Models;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Authentication.Cognito.Jwt.Handlers
{
    public class HasScope: AuthorizationHandler<HasScopeRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return;

            // Split the scopes string into an array
            var scopes = context
                .User
                .FindAll(c => 
                    c.Type == "scope" && 
                    c.Issuer == requirement.Issuer)
                .Select(claim => 
                    claim.Value)
                .ToList();

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement); 
        }
    }
}