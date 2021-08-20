using System.Collections.Generic;

namespace Expensely.Authentication.Cognito.Jwt.Options
{
    public class Auth
    {
        public string Issuer { get; set; }
        
        public string JwtKeySetUrl { get; set; }
        
        public List<string> Audiences { get; set; }
    }
}