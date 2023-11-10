using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class NotFoundAccountError : Error
{
    public NotFoundAccountError() 
        : base("Account not exists in current context")
    {
        Metadata.Add("ErrorCode", 400);
    }
}