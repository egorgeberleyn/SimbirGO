using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class UnauthorizedError : Error
{
    public UnauthorizedError() 
        : base("User unauthorized")
    {
        Metadata.Add("ErrorCode", 401);
    }
}