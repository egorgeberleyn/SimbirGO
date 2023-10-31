using FluentResults;

namespace Simbir.GO.Domain.Transports.Errors;

public class IncorrectTransportTypeError : Error
{
    public IncorrectTransportTypeError(string transportType) 
        : base($"[{transportType}] does not conform to existing transport types: Car, Bike, Scooter")
    {
        Metadata.Add("ErrorCode", 400);
    }
}