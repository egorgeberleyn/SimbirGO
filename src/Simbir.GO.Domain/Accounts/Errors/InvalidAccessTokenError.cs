using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class InvalidAccessTokenError : Error
{
    public InvalidAccessTokenError() 
        : base("Invalid access token")
    {
        Metadata.Add("ErrorCode", 401);
    }
}