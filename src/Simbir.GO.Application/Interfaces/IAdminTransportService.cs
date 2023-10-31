using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Transports;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Application.Interfaces;

public interface IAdminTransportService
{
    Task<Result<List<Transport>>> GetTransportsAsync(SelectTransportParams selectParams);
    Task<Result<Transport>> GetTransportAsync(long id);

    Task<Result<Success>> CreateTransportAsync(CreateTransportRequest request);
    Task<Result<Success>> UpdateTransportAsync(long id, AdminUpdateTransportRequest request);
    Task<Result<Success>> DeleteTransportAsync(long id);
}