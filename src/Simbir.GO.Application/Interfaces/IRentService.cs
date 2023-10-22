using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Domain.Accounts.Entities;

namespace Simbir.GO.Application.Interfaces;

public interface IRentService
{
    Task<List<AccountRent>> GetRentHistoryAsync();
    Task<AccountRent> GetTransportRentHistoryAsync();
    Task<long> StartRentAsync(long transportId, StartRentRequest request);
    Task<long> EndRentAsync(long rentId, EndRentRequest request);
}