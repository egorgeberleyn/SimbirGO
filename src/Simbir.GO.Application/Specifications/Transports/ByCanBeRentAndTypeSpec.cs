using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Enums;
using Simbir.GO.Domain.Transports.Services;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Transports;

public class ByCanBeRentAndTypeSpec : Specification<Transport>
{
    public ByCanBeRentAndTypeSpec(TransportType type) 
        : base(t => t.CanBeRented && t.TransportType == type)
    {
    }
}