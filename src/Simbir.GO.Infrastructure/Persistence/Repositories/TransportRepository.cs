using Microsoft.EntityFrameworkCore;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Infrastructure.Persistence.Repositories;

public class TransportRepository : ITransportRepository
{
    private readonly AppDbContext _dbContext;
    
    public TransportRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Transport?> FindTransportByIdAsync(long transportId) =>
        await _dbContext.Transports.FirstOrDefaultAsync(t => t.Id == transportId);

    public async Task AddTransportAsync(Transport transport)
    {
        await _dbContext.Transports.AddAsync(transport);
    }

    public void UpdateTransport(Transport updatedTransport)
    {
        _dbContext.Update(updatedTransport);
    }

    public void DeleteTransport(Transport transport)
    {
        _dbContext.Remove(transport);
    }
}