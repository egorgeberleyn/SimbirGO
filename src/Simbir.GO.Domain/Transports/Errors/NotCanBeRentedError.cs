using FluentResults;

namespace Simbir.GO.Domain.Transports.Errors;

public class NotCanBeRentedError : Error
{
    public NotCanBeRentedError() : base("Transport not can be rented")
    {
        Metadata.Add("ErrorCode", 400);
    }
}