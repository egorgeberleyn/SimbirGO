using Simbir.GO.Domain.Rents.Enums;

namespace Simbir.GO.Application.Contracts.Rents;

public record RentResponse(
    long TransportId,
    long AccountId,
    DateTime TimeStart,
    DateTime? TimeEnd,
    double PriceOfUnit,
    PriceType PriceType,
    double? FinalPrice);
