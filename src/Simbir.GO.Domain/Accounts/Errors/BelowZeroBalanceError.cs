using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class BelowZeroBalanceError : Error
{
    public BelowZeroBalanceError(double value) 
        : base($"Balance = {value} cannot be less than 0")
    {
        Metadata.Add("ErrorCode", 400);
    }
}