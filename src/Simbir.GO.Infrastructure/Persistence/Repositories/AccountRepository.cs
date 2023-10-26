using Simbir.GO.Domain.Accounts;
using Simbir.GO.Shared.Persistence.Repositories;

namespace Simbir.GO.Infrastructure.Persistence.Repositories;

public class AccountRepository : Repository<Account>
{
    private readonly AppDbContext _dbContext;
    
    public AccountRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}