using System;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Authentication.Cognito.Jwt.Models
{
    public class HasScope : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public HasScope(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}