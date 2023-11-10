using System.Net;
using Simbir.GO.Application.Interfaces.Auth;

namespace Simbir.GO.API.Middlewares;

public class JwtInvalidationMiddleware
{
    private readonly RequestDelegate _next;
    
    public JwtInvalidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IJwtTokenGenerator jwtTokenGenerator)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var header))
        {
            var authorizationHeader = header.ToString();
            var accessToken = authorizationHeader.Replace("bearer ", string.Empty);
            
            if (jwtTokenGenerator.IsInvalidToken(accessToken))
            { 
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized. The token has been revoked.");
                return;
            }
        }

        await _next(context);
    }
}