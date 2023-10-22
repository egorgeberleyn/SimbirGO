namespace Simbir.GO.Application.Contracts.Admin.Rents;

public record CreateRentRequest(
    long TransportId,
    long UserId,
    string TimeStart,
    string TimeEnd,
    double PriceOfUnit,
    string PriceType,
    double FinalPrice);
