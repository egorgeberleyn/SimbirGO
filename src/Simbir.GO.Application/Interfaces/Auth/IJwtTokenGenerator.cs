using System.Security.Claims;
using FluentResults;
using Simbir.GO.Application.Services.Common;
using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    Task<(string accessToken, string refreshToken)> GenerateTokenPairAsync(Account account);

    Result<Success> ValidateRefreshToken(ClaimsPrincipal tokenInVerification, ref RefreshToken refreshToken);

    Result<ClaimsPrincipal> ValidateAccessToken(string accessToken);
}