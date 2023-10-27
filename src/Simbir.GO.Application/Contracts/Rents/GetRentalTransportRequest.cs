namespace Simbir.GO.Application.Contracts.Rents;

public record GetRentalTransportRequest(
    double Lat, 
    double Long,
    double Radius,
    string Type);