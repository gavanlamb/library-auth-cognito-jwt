using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expensely.Authentication.Cognito.Jwt.Clients
{
    public interface IUserPoolClient
    {
        Task<IEnumerable<string>> GetOAuthScopes(
            string clientId);
    }
}