﻿using Simbir.GO.Domain.Transports;
using Simbir.GO.Shared.Persistence.Repositories;

namespace Simbir.GO.Infrastructure.Persistence.Repositories;

public class TransportRepository : Repository<Transport>
{
    private readonly AppDbContext _dbContext;
    
    public TransportRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}