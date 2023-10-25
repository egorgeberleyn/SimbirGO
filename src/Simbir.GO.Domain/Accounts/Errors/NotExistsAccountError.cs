using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class NotExistsAccountError : Error
{
    public NotExistsAccountError() 
        : base("Account not exists in current context")
    {
        Metadata.Add("ErrorCode", 400);
    }
}