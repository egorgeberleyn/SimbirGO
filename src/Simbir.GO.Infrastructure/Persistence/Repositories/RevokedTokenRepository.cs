using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Services.Common;
using Simbir.GO.Shared.Persistence.Repositories;

namespace Simbir.GO.Infrastructure.Persistence.Repositories;

public class RevokedTokenRepository : Repository<RevokedToken>, IRevokedTokenRepository
{
    private readonly AppDbContext _dbContext;
    
    public RevokedTokenRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}