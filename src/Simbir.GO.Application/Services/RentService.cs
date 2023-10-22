using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Domain.Accounts.Entities;

namespace Simbir.GO.Application.Services;

public class RentService : IRentService
{
    public Task<List<AccountRent>> GetRentHistoryAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AccountRent> GetTransportRentHistoryAsync()
    {
        throw new NotImplementedException();
    }

    public Task<long> StartRentAsync(long transportId, StartRentRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<long> EndRentAsync(long rentId, EndRentRequest request)
    {
        throw new NotImplementedException();
    }
}