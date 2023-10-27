using System.ComponentModel;
using FluentResults;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Rents.Enums;
using Simbir.GO.Domain.Rents.Errors;
using Simbir.GO.Domain.Transports;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Rents;

public class Rent : Entity
{
    public long TransportId { get; private set; }
    public long UserId { get; private set; }
    public DateTime TimeStart { get; init; }
    public DateTime? TimeEnd { get; private set; }
    public double PriceOfUnit { get; private set; }
    public PriceType PriceType { get; private set; }
    public double? FinalPrice { get; private set; }

    public Account Account { get; init; } = null!;
    public Transport Transport { get; init; } = null!;

    private Rent(long transportId, long userId, double priceOfUnit,
        PriceType priceType, DateTime timeStart, DateTime? timeEnd = null, double? finalPrice = null)
    {
        TransportId = transportId;
        UserId = userId;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
        PriceOfUnit = priceOfUnit;
        PriceType = priceType;
        FinalPrice = finalPrice;
    }

    public static Result<Rent> Create(long transportId, long userId, DateTime timeStart, DateTime? timeEnd,
        double priceOfUnit, string priceType, double? finalPrice)
    {
        if (!Enum.TryParse<PriceType>(priceType, true, out var rentPriceType))
            return Result.Fail(new IncorrectPriceTypeError(priceType));

        return new Rent(transportId, userId, priceOfUnit, rentPriceType, timeStart, timeEnd, finalPrice);
    }

    public static Result<Rent> Start(long transportId, long userId, double priceOfUnit,
        string priceType)
    {
        if (!Enum.TryParse<PriceType>(priceType, true, out var rentPriceType))
            return Result.Fail(new IncorrectPriceTypeError(priceType));

        return new Rent(transportId, userId, priceOfUnit, rentPriceType, timeStart: DateTime.UtcNow);
    }

    public Result<Rent> End()
    {
        var finalPrice = PriceType switch
        {
            PriceType.Minutes => 0,
            PriceType.Days => 1,
            PriceType.None => throw new InvalidEnumArgumentException($"{nameof(PriceType)} cannot be None"),
            _ => throw new ArgumentOutOfRangeException($"Argument {nameof(PriceType)} out of range")
        };

        FinalPrice = finalPrice;
        TimeEnd = DateTime.UtcNow;
        return this;
    }
}