using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Rents;
using Simbir.GO.Domain.Rents;

namespace Simbir.GO.Application.Interfaces;

public interface IAdminRentService
{
    Task<Result<Rent>> GetRentByIdAsync(long rentId);
    Task<Result<List<Rent>>> GetUserRentHistoryAsync(long accountId);
    Task<Result<List<Rent>>> GetTransportRentHistoryAsync(long transportId);
    
    Task<Result<long>> CreateRentAsync(CreateRentRequest request);
    Task<Result<long>> AdminEndRentAsync(long rentId, AdminEndRentRequest request);
    Task<Result<long>> UpdateRentAsync(long rentId, UpdateRentRequest request);
    Task<Result<long>> DeleteRentAsync(long rentId);
}