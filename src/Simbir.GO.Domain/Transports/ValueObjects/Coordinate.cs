using FluentResults;
using Simbir.GO.Domain.Transports.Errors;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Transports.ValueObjects;

public class Coordinate : ValueObject
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    private Coordinate(double latitude, double longitude) => 
        (Latitude, Longitude) = (latitude, longitude);

    public static Result<Coordinate> Create(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
            return new OutOfRangeLatitudeError(latitude);
        
        if (longitude is < -180 or > 180)
            return new OutOfRangeLongitudeError(longitude);
        
        return new Coordinate(latitude, longitude);
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Latitude;
        yield return Longitude;
    }
}