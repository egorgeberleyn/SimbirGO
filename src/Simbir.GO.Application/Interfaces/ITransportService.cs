using FluentResults;
using Simbir.GO.Application.Contracts.Transports;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Application.Interfaces;

public interface ITransportService
{
    Task<Result<Transport>> GetTransportByIdAsync(long transportId);
    
    Task<Result<Success>> AddTransportAsync(AddTransportRequest request);
    Task<Result<Success>> UpdateTransportAsync(long transportId, UpdateTransportRequest request);
    Task<Result<Success>> DeleteTransportAsync(long transportId);
}