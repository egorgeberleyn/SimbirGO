using Simbir.GO.Domain.Accounts;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications.Accounts;

public class SelectionFilterSpec : Specification<Account>
{
    public SelectionFilterSpec(int start, int count)
    {
        //AddSelection();
        //--->
        //AddSkip();
        //AddTake();
    }
}