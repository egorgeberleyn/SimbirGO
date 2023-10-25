using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class InvalidRefreshTokenError : Error
{
    public InvalidRefreshTokenError() 
        : base("Invalid refresh token")
    {
        Metadata.Add("ErrorCode", 401);
    }
}