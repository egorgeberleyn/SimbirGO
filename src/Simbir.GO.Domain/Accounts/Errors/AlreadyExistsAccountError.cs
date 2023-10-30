using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class AlreadyExistsAccountError : Error
{
    public AlreadyExistsAccountError(string username) 
        : base($"Account with username = {username} already exists")
    {
        Metadata.Add("ErrorCode", 400);
    }
}