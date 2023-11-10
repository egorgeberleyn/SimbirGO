using Simbir.GO.Domain.Rents;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Rents;

public class ByTransportSpec : Specification<Rent>
{
    public ByTransportSpec(long transportId) : base(ar => ar.TransportId == transportId)
    {
        AddOrderBy(t => t.TimeStart);
    }
}