namespace Simbir.GO.Application.Contracts.Rents;

public record SearchTransportParams(
    double Lat, 
    double Long,
    double Radius,
    string Type);