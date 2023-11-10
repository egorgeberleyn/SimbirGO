namespace Simbir.GO.Application.Contracts.Transports;

public record TransportResponse(
    long OwnerId,
    bool CanBeRented, 
    string TransportType, 
    string Model, 
    string Color, 
    string Identifier,
    string Description,
    double Latitude,
    double Longitude,
    double MinutePrice,
    double DayPrice);
