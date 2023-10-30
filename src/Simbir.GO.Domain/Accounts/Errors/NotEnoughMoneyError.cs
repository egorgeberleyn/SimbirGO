using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class NotEnoughMoneyError : Error
{
    public NotEnoughMoneyError() : base("Not enough money in the account balance")
    {
        Metadata.Add("ErrorCode", 400);
    }
}