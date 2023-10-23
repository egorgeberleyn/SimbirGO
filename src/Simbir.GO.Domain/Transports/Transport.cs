using FluentResults;
using Simbir.GO.Domain.Transports.Enums;
using Simbir.GO.Domain.Transports.Errors;
using Simbir.GO.Domain.Transports.ValueObjects;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Transports;

public class Transport : Entity
{
    public long OwnerId { get; private set; }
    public bool CanBeRented { get; private set; }
    public TransportType TransportType { get; private set; } 
    public string Model { get; private set; }
    public string Color { get; private set; }
    public string Identifier { get; private set; }
    public string? Description { get; private set; }
    public double? MinutePrice { get; private set; }
    public double? DayPrice { get; private set; }
    public Coordinate Coordinate { get; private set; }
    
    private Transport(long ownerId, bool canBeRented, TransportType transportType, string model, string color, 
        string identifier, string description, double? minutePrice, double? dayPrice, Coordinate coordinate)
    {
        OwnerId = ownerId;
        CanBeRented = canBeRented;
        TransportType = transportType;
        Model = model;
        Color = color;
        Identifier = identifier;
        Description = description;
        MinutePrice = minutePrice;
        DayPrice = dayPrice;
        Coordinate = coordinate;
    }

    public static Result<Transport> Create(long ownerId, bool canBeRented, string type, string model, string color,
        string identifier, string description, double? minutePrice, double? dayPrice, double latitude, double longitude)
    {
        var transportType = CheckTransportType(type);
        var newCoordinate = Coordinate.Create(latitude: latitude, longitude: longitude);
        if (newCoordinate.IsFailed)
            return Result.Fail(newCoordinate.Errors[0]);

        return new Transport(ownerId, canBeRented, transportType.Value, model, color, identifier,
            description, minutePrice, dayPrice, newCoordinate.Value);
    }
    
    public Result<Transport> Update(bool canBeRented, string type, string model, string color,
        string identifier, string description, double? minutePrice, double? dayPrice, double latitude, double longitude)
    {
        var transportType = CheckTransportType(type);
        var newCoordinate = Coordinate.Create(latitude: latitude, longitude: longitude);
        if (newCoordinate.IsFailed)
            return Result.Fail(newCoordinate.Errors[0]);

        CanBeRented = canBeRented;
        TransportType = transportType.Value;
        Model = model;
        Color = color;
        Identifier = identifier;
        Description = description;
        MinutePrice = minutePrice;
        DayPrice = dayPrice;
        Coordinate = newCoordinate.Value;

        return this;
    }

    private static Result<TransportType> CheckTransportType(string type)
    {
        return !Enum.TryParse<TransportType>(type, true, out var transportType) 
            ? Result.Fail(new IncorrectTransportTypeError(type)) 
            : Result.Ok(transportType);
    }
}