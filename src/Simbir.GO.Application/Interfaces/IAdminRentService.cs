using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Rents;
using Simbir.GO.Domain.Rents;

namespace Simbir.GO.Application.Interfaces;

public interface IAdminRentService
{
    Task<Result<Rent>> GetRentByIdAsync(long rentId);
    Task<Result<List<Rent>>> GetUserRentHistoryAsync(long accountId);
    Task<Result<List<Rent>>> GetTransportRentHistoryAsync(long transportId);
    
    Task<Result<Success>> CreateRentAsync(CreateRentRequest request);
    Task<Result<Success>> AdminEndRentAsync(long rentId, AdminEndRentRequest request);
    Task<Result<Success>> UpdateRentAsync(long rentId, UpdateRentRequest request);
    Task<Result<Success>> DeleteRentAsync(long rentId);
}