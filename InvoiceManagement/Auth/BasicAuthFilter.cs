using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace InvoiceManagement.Auth
{
    public class BasicAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string username = "";
            string password = "";
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(context.HttpContext.Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                username = credentials[0];
                password = credentials[1];
            }
            catch
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // dGVzdDp0ZXN0
            if (username != "test" || password != "test") // super secure
                context.Result = new UnauthorizedResult();
        }
    }
}
