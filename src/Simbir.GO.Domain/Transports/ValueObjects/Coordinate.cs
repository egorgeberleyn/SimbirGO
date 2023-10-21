using Simbir.GO.Shared.Entities;
using StatusGeneric;

namespace Simbir.GO.Domain.Transports.ValueObjects;

public class Coordinate : ValueObject
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }

    private Coordinate(double latitude, double longitude) => 
        (Latitude, Longitude) = (latitude, longitude);

    public static IStatusGeneric<Coordinate> Create(double latitude, double longitude)
    {
        var status = new StatusGenericHandler<Coordinate>();
        //Add validation
        return status.SetResult(new Coordinate(latitude, longitude));
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Latitude;
        yield return Longitude;
    }
}