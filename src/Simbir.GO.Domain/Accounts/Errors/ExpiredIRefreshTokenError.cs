using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class ExpiredIRefreshTokenError : Error
{
    public ExpiredIRefreshTokenError() 
        : base("Expired refresh token")
    {
        Metadata.Add("ErrorCode", 401);
    }
}