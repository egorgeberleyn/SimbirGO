using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Enums;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Transports;

public class ByCountFilterAndTypeSpec : Specification<Transport>
{
    public ByCountFilterAndTypeSpec(int start, int count, TransportType transportType) 
        : base(t => t.TransportType == transportType)
    {
        AddSkip(start == 0 
            ? 0 
            : start - 1);
        AddTake(count);
    }
    
    public ByCountFilterAndTypeSpec(int start, int count) 
    {
        AddSkip(start == 0 
            ? 0 
            : start - 1);
        AddTake(count);
    }
}