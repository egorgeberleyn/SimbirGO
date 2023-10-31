using FluentResults;

namespace Simbir.GO.Domain.Rents.Errors;

public class ExpiredRentError : Error
{
    public ExpiredRentError(DateTime timeEnd) : base($"Rent expired at {timeEnd}")
    {
        Metadata.Add("ErrorCode", 404);
    }
}