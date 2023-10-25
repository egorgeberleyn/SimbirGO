using Simbir.GO.Domain.Accounts;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Specifications;

public class ByUsernameSpec : Specification<Account>
{
    public ByUsernameSpec(string username) : base(a => a.Username == username)
    {
        
    }
}