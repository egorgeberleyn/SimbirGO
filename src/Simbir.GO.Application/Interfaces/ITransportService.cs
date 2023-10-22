using FluentResults;
using Simbir.GO.Application.Contracts.Transports;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Application.Interfaces;

public interface ITransportService
{
    Task<Result<Transport>> GetTransportByIdAsync(long transportId);
    Task<Result<long>> AddTransportAsync(AddTransportRequest request);
    Task<Result<long>> UpdateTransportAsync(long transportId, UpdateTransportRequest request);
    Task<Result<long>> DeleteTransportAsync(long transportId);
}