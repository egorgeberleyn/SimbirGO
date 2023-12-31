﻿namespace Simbir.GO.Application.Contracts.Admin.Transports;

public record CreateTransportRequest(
    long OwnerId,
    bool CanBeRented, 
    string TransportType, 
    string Model, 
    string Color, 
    string Identifier,
    string? Description,
    double Latitude,
    double Longitude,
    double? MinutePrice,
    double? DayPrice);