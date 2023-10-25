using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class ExpiredIAccessTokenError : Error
{
    public ExpiredIAccessTokenError()
        : base("Expired access token")
    {
        Metadata.Add("ErrorCode", 401);
    }
}