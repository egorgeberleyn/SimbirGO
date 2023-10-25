using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Services.Common;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Infrastructure.Persistence;

namespace Simbir.GO.Infrastructure.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AppDbContext _appDbContext;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, IDateTimeProvider dateTimeProvider,
        AppDbContext appDbContext)
    {
        _jwtSettings = jwtSettings.Value;
        _dateTimeProvider = dateTimeProvider;
        _appDbContext = appDbContext;
    }

    public async Task<(string accessToken, string refreshToken)> GenerateTokenPairAsync(Account account)
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

        var refreshToken = new RefreshToken()
        {
            JwtId = securityToken.Id,
            Token = GenerateRefreshToken(length: 23),
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            IsUsed = false,
            IsRevoked = false,
            UserId = account.Id
        };

        await _appDbContext.RefreshTokens.AddAsync(refreshToken);
        await _appDbContext.SaveChangesAsync();

        return (accessToken, refreshToken.Token);
    }


    public Result<Success> ValidateRefreshToken(
        ClaimsPrincipal tokenInVerification,
        ref RefreshToken refreshToken)
    {
        if (refreshToken.IsRevoked || refreshToken.IsUsed)
            return new InvalidRefreshTokenError();

        var jti = tokenInVerification.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        if (refreshToken.JwtId != jti)
            return new InvalidRefreshTokenError();

        if (refreshToken.ExpiryDate < DateTime.UtcNow)
            return new ExpiredIRefreshTokenError();

        return new Success("Token is valid");
    }

    public Result<ClaimsPrincipal> ValidateAccessToken(string accessToken)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false, //for dev
            ValidateAudience = false, //for dev
            RequireExpirationTime = false, // for dev -- needs to be updated when refresh token is added
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
        };

        var tokenInVerification = jwtTokenHandler.ValidateToken(accessToken, tokenValidationParams,
            out var validToken);

        if (validToken is JwtSecurityToken jwtSecurityToken)
        {
            var isValid = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);

            if (!isValid)
                return new InvalidAccessTokenError();
        }

        var utcExpiryDate = long.Parse(
            tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)!.Value);

        var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

        if (expiryDate > DateTime.Now)
            return new ExpiredIAccessTokenError();

        return tokenInVerification;
    }


    private static string GenerateRefreshToken(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

        return dateTimeVal;
    }
}