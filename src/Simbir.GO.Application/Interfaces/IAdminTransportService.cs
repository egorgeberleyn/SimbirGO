using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Transports;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Application.Interfaces;

public interface IAdminTransportService
{
    Task<Result<List<Transport>>> GetTransportsAsync(SelectTransportParams selectParams);
    Task<Result<Transport>> GetTransportAsync(long id);

    Task<Result<long>> CreateTransportAsync(CreateTransportRequest request);
    Task<Result<long>> UpdateTransportAsync(long id, UpdateTransportRequest request);
    Task<Result<long>> DeleteTransportAsync(long id);
}