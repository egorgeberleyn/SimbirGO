namespace Simbir.GO.Application.Contracts.Admin.Rents;

public record UpdateRentRequest(
    long TransportId,
    long UserId,
    string TimeStart,
    string TimeEnd,
    double PriceOfUnit,
    string PriceType,
    double FinalPrice);
