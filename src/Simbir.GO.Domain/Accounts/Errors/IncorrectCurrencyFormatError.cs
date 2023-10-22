using FluentResults;

namespace Simbir.GO.Domain.Accounts.Errors;

public class IncorrectCurrencyFormatError : Error
{
    public IncorrectCurrencyFormatError(string currency) 
        : base($"[{currency}] does not conform to existing currency formats")
    {
        Metadata.Add("ErrorCode", 400);
    }
}