using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class NoRightsAccountError : Error
{
    public NoRightsAccountError() : base("Account has no rights to perform the action")
    {
        Metadata.Add("ErrorCode", 403);
    }
}