using Microsoft.EntityFrameworkCore;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Rents;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Application.Interfaces.Persistence;

public interface IAppDbContext
{
    DbSet<Account> Accounts { get; set; }
    DbSet<Rent> Rents { get; set; } 
    DbSet<Transport> Transports { get; set; } 
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}