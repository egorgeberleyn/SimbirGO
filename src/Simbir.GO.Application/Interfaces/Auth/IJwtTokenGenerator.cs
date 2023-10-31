using System.IdentityModel.Tokens.Jwt;
using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(Account account);
    JwtSecurityToken ParseToken(string tokenStr);
    bool IsInvalidToken(string tokenStr);
}