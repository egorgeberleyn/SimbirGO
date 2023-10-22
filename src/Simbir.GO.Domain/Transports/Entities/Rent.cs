using FluentResults;
using Simbir.GO.Domain.Transports.Enums;
using Simbir.GO.Domain.Transports.Errors;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Transports.Entities;

public class Rent : Entity
{
    public long TransportId { get; private set; }
    public long UserId { get; private set; }
    public DateTime TimeStart { get; init; }
    public DateTime? TimeEnd { get; private set; }
    public double PriceOfUnit { get; private set; }
    public PriceType PriceType { get; private set; }
    public double? FinalPrice { get; private set; }

    private Rent(long transportId, long userId, DateTime? timeEnd, double priceOfUnit, 
        PriceType priceType, double? finalPrice)
    {
        TransportId = transportId;
        UserId = userId;
        TimeStart = DateTime.UtcNow;
        TimeEnd = timeEnd;
        PriceOfUnit = priceOfUnit;
        PriceType = priceType;
        FinalPrice = finalPrice;
    }

    public static Result<Rent> Create(long transportId, long userId, DateTime? timeEnd, double priceOfUnit,
        string priceType, double? finalPrice)
    {
        if (!Enum.TryParse<PriceType>(priceType, true, out var rentPriceType))
            return Result.Fail(new IncorrectPriceTypeError(priceType));
            
        return new Rent(transportId, userId, timeEnd, priceOfUnit, rentPriceType, finalPrice);
    }
}