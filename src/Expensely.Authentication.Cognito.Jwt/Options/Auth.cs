using System.Collections.Generic;

namespace Expensely.Authentication.Cognito.Jwt.Options
{
    public class Auth
    {
        public string Issuer { get; set; }
        
        public string JwtKeySetUrl { get; set; }
        
        public IEnumerable<string> Audiences { get; set; }
        
        public IEnumerable<string> Scopes { get; set; }
    }
}