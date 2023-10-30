using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class InvalidCredentialsError : Error
{
    public InvalidCredentialsError() : base("Invalid credentials")
    {
        Metadata.Add("ErrorCode", 401);
    }
}