using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class IncorrectRoleError : Error
{
    public IncorrectRoleError() : base("Incorrect role. Must be in range [None, Client, Admin]")
    {
        Metadata.Add("ErrorCode", 400);
    }
}