using System.Linq;

namespace Expensely.Authentication.Cognito.Jwt.Extensions
{
    public static class ClaimsPrincipal
    {
        public static string GetSubject(this System.Security.Claims.ClaimsPrincipal principal) => principal.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
    }
}