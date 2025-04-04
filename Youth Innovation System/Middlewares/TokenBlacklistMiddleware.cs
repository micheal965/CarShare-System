using System.Net;
using Youth_Innovation_System.Core.IServices.Identity;

namespace Youth_Innovation_System.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            var token = context.Request.Headers["authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            var authservice = context.RequestServices.GetRequiredService<IAuthService>();
            if (!string.IsNullOrEmpty(token) && await authservice.IsTokenBlacklistedAsync(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized; // Unauthorized
                await context.Response.WriteAsync("Token is invalid or expired.");
                return;
            }

            await _next(context);
        }
    }
}
