using System.Net;
using Simbir.GO.Application.Interfaces.Auth;

namespace Simbir.GO.API.Middlewares;

public class JwtInvalidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public JwtInvalidationMiddleware(RequestDelegate next, IJwtTokenGenerator jwtTokenGenerator)
    {
        _next = next;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var header))
        {
            var authorizationHeader = header.ToString();
            var accessToken = authorizationHeader.Replace("Bearer ", string.Empty);
            
            if (_jwtTokenGenerator.IsInvalidToken(accessToken))
            { 
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized. The token has been revoked.");
                return;
            }
        }

        await _next(context);
    }
}