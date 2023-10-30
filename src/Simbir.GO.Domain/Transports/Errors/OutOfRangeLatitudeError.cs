using FluentResults;

namespace Simbir.GO.Domain.Transports.Errors;

public class OutOfRangeLatitudeError : Error
{
    public OutOfRangeLatitudeError(double latitude) 
        : base($"Latitude = [{latitude}] must be between -90 and 90 degrees inclusive.")
    {
        Metadata.Add("ErrorCode", 400);
    }
}