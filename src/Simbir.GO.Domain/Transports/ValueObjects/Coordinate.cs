using FluentResults;
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
        //Add validation
        return new Coordinate(latitude, longitude);
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Latitude;
        yield return Longitude;
    }
}