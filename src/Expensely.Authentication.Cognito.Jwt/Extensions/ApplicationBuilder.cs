using Microsoft.AspNetCore.Builder;

namespace Expensely.Authentication.Cognito.Jwt.Extensions
{
    public static class ApplicationBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static void UseAuth(
            this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}