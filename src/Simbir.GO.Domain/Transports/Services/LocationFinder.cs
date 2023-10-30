using Simbir.GO.Domain.Transports.ValueObjects;

namespace Simbir.GO.Domain.Transports.Services;

public class LocationFinder
{
    public double CalculateDistance(double searchLatitude, double searchLongitude, Coordinate transportCoordinate)
    {
        var latitudeDifference = Math.Abs(searchLatitude - transportCoordinate.Latitude);
        var longitudeDifference = Math.Abs(searchLongitude - transportCoordinate.Longitude);
        var distance = Math.Sqrt(Math.Pow(latitudeDifference, 2) + Math.Pow(longitudeDifference, 2));

        return distance;
    }
}