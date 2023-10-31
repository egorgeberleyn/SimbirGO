using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Enums;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Transports;

public class ByCountFilterAndTypeSpec : Specification<Transport>
{
    public ByCountFilterAndTypeSpec(int start, int count, TransportType transportType) 
        : base(t => t.TransportType == transportType)
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