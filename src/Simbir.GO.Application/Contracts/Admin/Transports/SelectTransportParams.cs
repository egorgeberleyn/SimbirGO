namespace Simbir.GO.Application.Contracts.Admin.Transports;

public record SelectTransportParams(
    int Start,
    int Count,
    string TransportType);