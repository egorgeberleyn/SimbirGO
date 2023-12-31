﻿using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Domain.Rents;
using Simbir.GO.Shared.Persistence.Repositories;

namespace Simbir.GO.Infrastructure.Persistence.Repositories;

public class RentRepository : Repository<Rent>, IRentRepository
{
    private readonly AppDbContext _dbContext;
    
    public RentRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}