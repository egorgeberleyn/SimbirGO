using FluentResults;
using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Domain.Rents;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Application.Interfaces;

public interface IRentService
{
    Task<Result<List<Transport>>> GetRentalTransportAsync(SearchTransportParams request);
    Task<Result<Rent>> GetRentByIdAsync(long rentId);
    Task<Result<List<Rent>>> GetMyRentHistoryAsync();
    Task<Result<List<Rent>>> GetTransportRentHistoryAsync(long transportId);
    
    Task<Result<long>> StartRentAsync(long transportId, StartRentRequest request);
    Task<Result<long>> EndRentAsync(long rentId, EndRentRequest request);
}