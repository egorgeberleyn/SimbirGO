using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Infrastructure.Persistence;

namespace Simbir.GO.Infrastructure.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AppDbContext _dbContext;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, IDateTimeProvider dateTimeProvider, 
        AppDbContext dbContext)
    {
        _jwtSettings = jwtSettings.Value;
        _dateTimeProvider = dateTimeProvider;
        _dbContext = dbContext;
    }

    public string GenerateToken(Account account)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, account.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime()
                .ToString(CultureInfo.CurrentCulture)),
            new Claim(ClaimTypes.Role, account.Role.ToString()),
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return accessToken;
    }

    public JwtSecurityToken ParseToken(string tokenStr)
    {
        var jwtToken = new JwtSecurityToken(tokenStr);
        return jwtToken;
    }

    public bool IsInvalidToken(string tokenStr)
    {
        var jwtToken = new JwtSecurityToken(tokenStr);
        var token = _dbContext.RevokedTokens
            .FirstOrDefault(token => token.IsRevoked && token.JwtId == jwtToken.Id);
        return token is not null;
    }
}