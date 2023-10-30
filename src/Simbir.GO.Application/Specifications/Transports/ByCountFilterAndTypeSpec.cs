using Simbir.GO.Domain.Transports;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Transports;

public class ByCountFilterAndTypeSpec : Specification<Transport>
{
    public ByCountFilterAndTypeSpec(int start, int count, string transportType) 
        : base(t => t.TransportType.ToString().Equals(transportType, StringComparison.OrdinalIgnoreCase))
    {
        AddSkip(start);
        AddTake(count);
    }
    
    public ByCountFilterAndTypeSpec(int start, int count) 
    {
        AddSkip(start);
        AddTake(count);
    }
}