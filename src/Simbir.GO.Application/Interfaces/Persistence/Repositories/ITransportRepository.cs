using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Application.Interfaces.Persistence.Repositories;

public interface ITransportRepository
{
    Task<Transport?> FindTransportByIdAsync(long transportId);
    Task AddTransportAsync(Transport transport);
    void UpdateTransport(Transport updatedTransport);
    void DeleteTransport(Transport transport);
}