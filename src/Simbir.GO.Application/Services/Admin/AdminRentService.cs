using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Rents;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Specifications.Rents;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Domain.Rents;
using Simbir.GO.Domain.Rents.Errors;
using Simbir.GO.Domain.Transports.Errors;
using Simbir.GO.Domain.Transports.ValueObjects;

namespace Simbir.GO.Application.Services.Admin;

public class AdminRentService : IAdminRentService
{
    private readonly IAppDbContext _dbContext;
    private readonly IRentRepository _rentRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransportRepository _transportRepository;

    public AdminRentService(IAppDbContext dbContext, IRentRepository rentRepository, 
        ITransportRepository transportRepository, IAccountRepository accountRepository)
    {
        _dbContext = dbContext;
        _rentRepository = rentRepository;
        _transportRepository = transportRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Rent>> GetRentByIdAsync(long rentId)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        return rent is null 
            ? new NotFoundRentError()
            : rent;
    }

    public async Task<Result<List<Rent>>> GetUserRentHistoryAsync(long accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account is null)
            return new NotFoundAccountError();

        var byAccountSpec = new ByAccountSpec(accountId);
        var accountRents = await _rentRepository.GetAllByAsync(byAccountSpec);

        return accountRents;
    }

    public async Task<Result<List<Rent>>> GetTransportRentHistoryAsync(long transportId)
    {
        var transport = await _transportRepository.GetByIdAsync(transportId);
        if (transport is null)
            return new NotFoundTransportError();

        var byTransportSpec = new ByTransportSpec(transportId);
        var transportRents = await _rentRepository.GetAllByAsync(byTransportSpec);

        return transportRents;
    }

    public async Task<Result<long>> CreateRentAsync(CreateRentRequest request)
    {
        var transport = await _transportRepository.GetByIdAsync(request.TransportId);
        if (transport is null)
            return new NotFoundTransportError();
        
        var startedRent = Rent.Create(transport.Id, request.UserId, request.TimeStart, request.TimeEnd,
            request.PriceOfUnit, request.PriceType, request.FinalPrice);
        if(startedRent.IsFailed)
            return Result.Fail(startedRent.Errors);

        await _rentRepository.AddAsync(startedRent.Value);
        await _dbContext.SaveChangesAsync();
        return startedRent.Value.Id;
    }

    public async Task<Result<long>> AdminEndRentAsync(long rentId, AdminEndRentRequest request)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if (rent is null)
            return new NotFoundRentError();

        if (await _transportRepository.GetByIdAsync(rent.TransportId) is not { } rentedTransport)
            return new NotFoundTransportError();
        
        var (_, isFailed, endedRent, errors) = rent.End();
        if (isFailed) return Result.Fail(errors);
        _rentRepository.Update(endedRent);
        
        //Update transport location
        var newCoordinate = Coordinate.Create(request.Lat, request.Long);
        if(newCoordinate.IsFailed)
            return Result.Fail(newCoordinate.Errors);
        rentedTransport.SetLocation(newCoordinate.Value);
        _transportRepository.Update(rentedTransport);
        
        _rentRepository.Update(endedRent);
        await _dbContext.SaveChangesAsync();
        return endedRent.Id;
    }

    public async Task<Result<long>> UpdateRentAsync(long rentId, UpdateRentRequest request)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if (rent is null)
            return new NotFoundRentError();

        var updatedRent = rent.Update(request.TransportId, request.UserId, request.TimeStart, request.TimeEnd,
            request.PriceOfUnit, request.PriceType, request.FinalPrice);
        if(updatedRent.IsFailed)
            return Result.Fail(updatedRent.Errors);
        
        _rentRepository.Update(updatedRent.Value);
        await _dbContext.SaveChangesAsync();
        return updatedRent.Value.Id;
    }

    public async Task<Result<long>> DeleteRentAsync(long rentId)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if (rent is null)
            return new NotFoundRentError();
        
        _rentRepository.Delete(rent);
        await _dbContext.SaveChangesAsync();
        return rent.Id;
    }
}