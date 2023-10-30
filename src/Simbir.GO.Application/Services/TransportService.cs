using FluentResults;
using Simbir.GO.Application.Contracts.Transports;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Errors;

namespace Simbir.GO.Application.Services;

public sealed class TransportService : ITransportService
{
    private readonly ITransportRepository _transportRepository;
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;

    public TransportService(ITransportRepository transportRepository, IAppDbContext dbContext, 
        ICurrentUserContext currentUserContext)
    {
        _transportRepository = transportRepository;
        _dbContext = dbContext;
        _currentUserContext = currentUserContext;
    }

    public async Task<Result<Transport>> GetTransportByIdAsync(long transportId)
    {
        var transport = await _transportRepository.GetByIdAsync(transportId);
        return transport is null
            ? Result.Fail(new NotFoundTransportError())
            : Result.Ok(transport);
    }

    public async Task<Result<long>> AddTransportAsync(AddTransportRequest request)
    {
        if(_currentUserContext.TryGetUserId(out var ownerId))
            return new NotFoundAccountError();
        
        var createdTransport = Transport.Create(ownerId, request.CanBeRented, request.TransportType, request.Model, 
            request.Color, request.Identifier, request.Description, request.MinutePrice, request.DayPrice,
            request.Latitude, request.Longitude);
        if (createdTransport.IsFailed)
            return Result.Fail(createdTransport.Errors);

        await _transportRepository.AddAsync(createdTransport.Value);
        await _dbContext.SaveChangesAsync();
        return createdTransport.Value.Id;
    }

    public async Task<Result<long>> UpdateTransportAsync(long transportId, UpdateTransportRequest request)
    {
        if(_currentUserContext.TryGetUserId(out var ownerId))
            return new NotFoundAccountError();
        
        var transport = await _transportRepository.GetByIdAsync(transportId);
        if(transport is null)
            return new NotFoundTransportError();

        var ownResult = transport.CheckOwner(ownerId);
        if (ownResult.IsFailed)
            return Result.Fail(ownResult.Errors);
        
        var updatedTransport = transport.Update(request.CanBeRented, request.TransportType, request.Model,
            request.Color, request.Identifier, request.Description, request.MinutePrice, request.DayPrice,
            request.Latitude, request.Longitude);
        if (updatedTransport.IsFailed)
            return Result.Fail(updatedTransport.Errors);

        _transportRepository.Update(updatedTransport.Value);
        await _dbContext.SaveChangesAsync();
        return updatedTransport.Value.Id;
    }

    public async Task<Result<long>> DeleteTransportAsync(long transportId)
    {
        if(_currentUserContext.TryGetUserId(out var ownerId))
            return new NotFoundAccountError();
        
        var transport = await _transportRepository.GetByIdAsync(transportId);
        if(transport is null)
            return new NotFoundTransportError();

        var ownResult = transport.CheckOwner(ownerId);
        if (ownResult.IsFailed)
            return Result.Fail(ownResult.Errors);
        
        _transportRepository.Delete(transport);
        await _dbContext.SaveChangesAsync();
        return transport.Id;
    }
}