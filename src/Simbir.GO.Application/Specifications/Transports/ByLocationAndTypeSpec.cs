using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Services;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Transports;

public class ByLocationAndTypeSpec : Specification<Transport>
{
    public ByLocationAndTypeSpec(LocationFinder locationFinder, 
        SearchTransportParams searchParams) 
        : base(t => t.CanBeRented
            && locationFinder.CalculateDistance(searchParams.Lat, searchParams.Long, t.Coordinate) <= searchParams.Radius
            && t.TransportType.ToString().Equals(searchParams.Type, StringComparison.OrdinalIgnoreCase))
    {
    }
}