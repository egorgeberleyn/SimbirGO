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
    public DateTime TimeStart { get; private set; }
    public DateTime? TimeEnd { get; private set; }
    public double PriceOfUnit { get; private set; }
    public PriceType PriceType { get; private set; }
    public double? FinalPrice { get; private set; }

    public Account Account { get; set; } = null!;
    public Transport Transport { get; set; } = null!;

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

    public static Result<Rent> Create(long transportId, long userId, string timeStart, string? timeEnd,
        double priceOfUnit, string priceType, double? finalPrice)
    {
        var (_, isFailed, validProps, errors) = Validate(priceType, timeStart, timeEnd);
        if (isFailed)
            return Result.Fail(errors);
        
        return new Rent(transportId, userId, priceOfUnit, validProps.PriceType, validProps.TimeStart, 
            validProps.TimeEnd, finalPrice);
    }
    
    public Result<Rent> Update(long transportId, long userId, string timeStart, string? timeEnd,
        double priceOfUnit, string priceType, double? finalPrice)
    {
        var (_, isFailed, validProps, errors) = Validate(priceType, timeStart, timeEnd);
        if (isFailed)
            return Result.Fail(errors);

        TransportId = transportId;
        UserId = userId;
        PriceOfUnit = priceOfUnit;
        PriceType = validProps.PriceType;
        TimeStart = validProps.TimeStart;
        TimeEnd = validProps.TimeEnd;
        FinalPrice = finalPrice;
        return this;
    }

    public static Result<Rent> Start(long transportId, long userId, string priceType, 
        double? dayPrice, double? minutePrice)
    {
        if (!Enum.TryParse<PriceType>(priceType, true, out var rentPriceType))
            return Result.Fail(new IncorrectPriceTypeError(priceType));

        var priceOfUnit = rentPriceType switch
        {
            PriceType.Minutes => minutePrice,
            PriceType.Days => dayPrice,
            PriceType.None => throw new InvalidEnumArgumentException($"{nameof(PriceType)} cannot be None"),
            _ => throw new ArgumentOutOfRangeException($"Argument {nameof(PriceType)} out of range")
        };
        if (priceOfUnit == null)
            return new NotIndicatedRentCostError();
        
        return new Rent(transportId, userId, priceOfUnit.Value, rentPriceType, timeStart: DateTime.UtcNow);
    }

    public Result<Rent> End()
    {
        TimeEnd = DateTime.UtcNow;
        var time = TimeEnd - TimeStart;
        
        var finalPrice = PriceType switch
        {
            PriceType.Minutes => time.Value.Minutes * PriceOfUnit,
            PriceType.Days => time.Value.Days * PriceOfUnit,
            PriceType.None => throw new InvalidEnumArgumentException($"{nameof(PriceType)} cannot be None"),
            _ => throw new ArgumentOutOfRangeException($"Argument {nameof(PriceType)} out of range")
        };

        FinalPrice = finalPrice;
        return this;
    }

    private record ValidateProps(PriceType PriceType, DateTime TimeStart, DateTime TimeEnd);
    private static Result<ValidateProps> Validate(string priceType, string timeStart, string? timeEnd)
    {
        if (!Enum.TryParse<PriceType>(priceType, true, out var rentPriceType))
            return Result.Fail(new IncorrectPriceTypeError(priceType));

        if (!DateTime.TryParse(timeStart, out var rentTimeStart))
            return Result.Fail(new IncorrectTimeFormatError());
        
        if (!DateTime.TryParse(timeEnd, out var rentTimeEnd))
            return Result.Fail(new IncorrectTimeFormatError());

        return new ValidateProps(rentPriceType, rentTimeStart, rentTimeEnd);
    }
}