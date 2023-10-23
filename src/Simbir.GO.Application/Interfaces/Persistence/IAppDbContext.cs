namespace Simbir.GO.Application.Interfaces.Persistence;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}