using Simbir.GO.Domain.Accounts;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Accounts;

public class ByCountFilterSpec : Specification<Account>
{
    public ByCountFilterSpec(int start, int count)
    {
        AddSkip(start);
        AddTake(count);
    }
}