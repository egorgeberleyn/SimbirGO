using FluentResults;

namespace Simbir.GO.Domain.Transports.Errors;

public class OutOfRangeLongitudeError : Error
{
    public OutOfRangeLongitudeError(double longitude) 
        : base($"Longitude = [{longitude}] must be between -180 and 180 degrees inclusive.")
    {
        Metadata.Add("ErrorCode", 400);
    }
}