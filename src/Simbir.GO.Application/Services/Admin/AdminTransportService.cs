using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Transports;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Specifications.Transports;
using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Enums;
using Simbir.GO.Domain.Transports.Errors;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Services.Admin;

public class AdminTransportService : IAdminTransportService
{
    private readonly IAppDbContext _dbContext;
    private readonly ITransportRepository _transportRepository;

    public AdminTransportService(IAppDbContext dbContext, ITransportRepository transportRepository)
    {
        _dbContext = dbContext;
        _transportRepository = transportRepository;
    }

    public async Task<Result<List<Transport>>> GetTransportsAsync(SelectTransportParams selectParams)
    {
        Specification<Transport> byCountFilterAndTypeSpec;
        if(selectParams.TransportType.Equals(nameof(TransportType.All), StringComparison.OrdinalIgnoreCase))
            byCountFilterAndTypeSpec = new ByCountFilterAndTypeSpec(selectParams.Start, selectParams.Count, 
                selectParams.TransportType);
        else
            byCountFilterAndTypeSpec = new ByCountFilterAndTypeSpec(selectParams.Start, selectParams.Count);
        return await _transportRepository.GetAllByAsync(byCountFilterAndTypeSpec);
    }

    public async Task<Result<Transport>> GetTransportAsync(long id)
    {
        var transport = await _transportRepository.GetByIdAsync(id);
        if (transport is null)
            return new NotFoundTransportError();

        return transport;
    }

    public async Task<Result<long>> CreateTransportAsync(CreateTransportRequest request)
    {
        var createdTransport = Transport.Create(request.OwnerId, request.CanBeRented, request.TransportType,
            request.Model, request.Color, request.Identifier, request.Description, 
            request.MinutePrice, request.DayPrice, request.Latitude, request.Longitude);
        if (createdTransport.IsFailed)
            return Result.Fail(createdTransport.Errors);

        await _transportRepository.AddAsync(createdTransport.Value);
        await _dbContext.SaveChangesAsync();
        return createdTransport.Value.Id;
    }

    public async Task<Result<long>> UpdateTransportAsync(long id, AdminUpdateTransportRequest request)
    {
        var transport = await _transportRepository.GetByIdAsync(id);
        if (transport is null)
            return new NotFoundTransportError();

        var updatedTransport = transport.Update(request.CanBeRented, request.TransportType,
            request.Model, request.Color, request.Identifier, request.Description,
            request.MinutePrice, request.DayPrice, request.Latitude, request.Longitude);
        
       _transportRepository.Update(updatedTransport.Value);
        await _dbContext.SaveChangesAsync();
        return updatedTransport.Value.Id;
    }

    public async Task<Result<long>> DeleteTransportAsync(long id)
    {
        var transport = await _transportRepository.GetByIdAsync(id);
        if (transport is null)
            return new NotFoundTransportError();
        
        _transportRepository.Delete(transport);
        await _dbContext.SaveChangesAsync();
        return transport.Id;
    }
}