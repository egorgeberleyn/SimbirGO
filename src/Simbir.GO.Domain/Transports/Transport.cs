using Simbir.GO.Domain.Transports.Enums;
using Simbir.GO.Domain.Transports.ValueObjects;
using Simbir.GO.Shared.Entities;
using StatusGeneric;

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

    public static IStatusGeneric<Transport> Create(long ownerId, bool canBeRented, string type, string model, string color,
        string identifier, string description, double? minutePrice, double? dayPrice, double latitude, double longitude)
    {
        var status = new StatusGenericHandler<Transport>();

        if (!Enum.TryParse<TransportType>(type, true, out var transportType))
            status.AddError("Incorrect transport type", nameof(type));

        var createdCoordinate = Coordinate.Create(latitude: latitude, longitude: longitude);
        if (status.CombineStatuses(createdCoordinate).HasErrors)
            return status;

        return status.SetResult(new Transport(ownerId, canBeRented, transportType, model, color, identifier,
            description, minutePrice, dayPrice, createdCoordinate.Result));
    }
}