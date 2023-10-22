using FluentResults;
using Simbir.GO.Application.Contracts.Transports;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Errors;

namespace Simbir.GO.Application.Services;

public class TransportService : ITransportService
{
    private readonly IAppDbContext _dbContext;

    public TransportService(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Transport>> GetTransportByIdAsync(long transportId)
    {
        var transport = await _dbContext.Transports.FindAsync(transportId);
        if(transport is null)
            return new NotFoundTransportError();

        return transport;
    }

    public async Task<Result<long>> AddTransportAsync(AddTransportRequest request)
    {
        var createdTransport = Transport.Create(0, request.CanBeRented, request.TransportType, request.Model, 
            request.Color, request.Identifier, request.Description, request.MinutePrice, request.DayPrice,
            request.Latitude, request.Longitude);
        if (createdTransport.IsFailed)
            return Result.Fail(createdTransport.Errors[0]);

        await _dbContext.Transports.AddAsync(createdTransport.Value);
        await _dbContext.SaveChangesAsync();
        return createdTransport.Value.Id;
    }

    public async Task<Result<long>> UpdateTransportAsync(long transportId, UpdateTransportRequest request)
    {
        var transport = await _dbContext.Transports.FindAsync(transportId);
        if (transport is null)
            return new NotFoundTransportError();

        var updatedTransport = transport.Update(request.CanBeRented, request.TransportType, request.Model,
            request.Color, request.Identifier, request.Description, request.MinutePrice, request.DayPrice,
            request.Latitude, request.Longitude);
        if (updatedTransport.IsFailed)
            return Result.Fail(updatedTransport.Errors[0]);

        _dbContext.Transports.Update(updatedTransport.Value);
        await _dbContext.SaveChangesAsync();
        return updatedTransport.Value.Id;
    }

    public async Task<Result<long>> DeleteTransportAsync(long transportId)
    {
        var transport = await _dbContext.Transports.FindAsync(transportId);
        if (transport is null)
            return new NotFoundTransportError();
        
        _dbContext.Transports.Remove(transport);
        await _dbContext.SaveChangesAsync();
        return transport.Id;
    }
}