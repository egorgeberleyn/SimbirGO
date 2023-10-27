using Simbir.GO.Domain.Rents;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Rents;

public class ByAccountSpec : Specification<Rent>
{
    public ByAccountSpec(long accountId) : base(ar => ar.UserId == accountId)
    {
        
    }
}