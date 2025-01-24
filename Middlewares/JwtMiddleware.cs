using mapa_back.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace mapa_back.Middlewares
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly JwtHelper _jwtSecurityTokenHandler;

        public JwtMiddleware(JwtHelper jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Get the token from the Authorization header
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //Console.WriteLine(context.Request.Headers["Authorization"].ToString());
            if (!string.IsNullOrEmpty(token))
            {

                try
                {
                    // Verify the token using the JwtSecurityTokenHandlerWrapper
                    var claimsPrincipal = _jwtSecurityTokenHandler.ValidateJwtToken(token);

                    // Extract the user ID from the token
                    var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    // Store the user ID in the HttpContext items for later use
                    context.Items["UserId"] = userId;

                    // You can also do the for same other key which you have in JWT token.
                }
                catch (Exception)
                {
                    // If the token is invalid, throw an exception
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                }


            }
            // Continue processing the request
            await next(context);
        }
    }
}
