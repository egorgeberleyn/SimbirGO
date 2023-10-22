using FluentResults;

namespace Simbir.GO.Domain.Transports.Errors;

public class IncorrectPriceTypeError : Error
{
    public IncorrectPriceTypeError(string priceType) : base($"[{priceType}] does not conform to existing price types")
    {
        Metadata.Add("ErrorCode", 400);
    }
}