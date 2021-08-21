using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Authentication.Cognito.Jwt.Models;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Authentication.Cognito.Jwt.Handlers
{
    public class HasScope: AuthorizationHandler<Models.HasScope>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, Models.HasScope requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return;

            // Split the scopes string into an array
            var claims = context
                .User
                .FindAll(c => 
                    c.Type == "scope" && 
                    c.Issuer == requirement.Issuer);

            var scopes = new List<string>();

            foreach (var claim in claims)
            {
                scopes.AddRange(claim.Value.Split(" "));
            }

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement); 
        }
    }
}