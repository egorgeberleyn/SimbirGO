using FluentResults;
using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Specifications.Rents;
using Simbir.GO.Application.Specifications.Transports;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Domain.Rents;
using Simbir.GO.Domain.Rents.Errors;
using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Errors;
using Simbir.GO.Domain.Transports.Services;
using Simbir.GO.Domain.Transports.ValueObjects;

namespace Simbir.GO.Application.Services;

public class RentService : IRentService
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IRentRepository _rentRepository;
    private readonly ITransportRepository _transportRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly LocationFinder _locationFinder;
   

    public RentService(IRentRepository rentRepository, ICurrentUserContext currentUserContext, 
        ITransportRepository transportRepository, IAppDbContext dbContext, LocationFinder locationFinder, 
        IAccountRepository accountRepository)
    {
        _rentRepository = rentRepository;
        _currentUserContext = currentUserContext;
        _transportRepository = transportRepository;
        _dbContext = dbContext;
        _locationFinder = locationFinder;
        _accountRepository = accountRepository;
    }

    public async Task<Result<List<Transport>>> GetRentalTransportAsync(SearchTransportParams searchParams)
    {
        var byLocationAndTypeSpec = new ByLocationAndTypeSpec(_locationFinder, searchParams);
        var availableTransports = await _transportRepository.GetAllByAsync(byLocationAndTypeSpec);
        return availableTransports;
    }

    public async Task<Result<Rent>> GetRentByIdAsync(long rentId)
    {
        if(_currentUserContext.TryGetUserId(out var userId))
            return new NotFoundAccountError();
        
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if(rent is null)
            return new NotFoundRentError();

        var rentedTransport = await _transportRepository.GetByIdAsync(rent.TransportId);
        if (rentedTransport is null)
            return new NotFoundTransportError();
            
        if(rent.UserId != userId || rentedTransport.OwnerId != userId)
            return new NoAccessToRentError();
            
        return rent;
    }

    public async Task<Result<List<Rent>>> GetMyRentHistoryAsync()
    {
        if(_currentUserContext.TryGetUserId(out var userId))
            return new NotFoundAccountError();

        var byAccountSpec = new ByAccountSpec(userId);
        var accountRents = await _rentRepository.GetAllByAsync(byAccountSpec);

        return accountRents;
    }

    public async Task<Result<List<Rent>>> GetTransportRentHistoryAsync(long transportId)
    {
        if(_currentUserContext.TryGetUserId(out var userId))
            return new NotFoundAccountError();
        
        var transport = await _transportRepository.GetByIdAsync(transportId);
        if (transport is null)
            return new NotFoundTransportError();

        if (userId != transport.OwnerId)
            return new NoAccessToTransportError();

        var byTransportSpec = new ByTransportSpec(transportId);
        var transportRents = await _rentRepository.GetAllByAsync(byTransportSpec);

        return transportRents;
    }

    public async Task<Result<long>> StartRentAsync(long transportId, StartRentRequest request)
    {
        if (!_currentUserContext.TryGetUserId(out var accountId))
            return new NotFoundAccountError();
        
        var transport = await _transportRepository.GetByIdAsync(transportId);
        if (transport is null)
            return new NotFoundTransportError();

        if (!transport.CanBeRented)
            return new NotCanBeRentedError();

        if (transport.OwnerId == accountId)
            return new RentOfOwnTransportError();
        
        var startedRent = Rent.Start(transportId, accountId, request.RentType, 
            transport.DayPrice, transport.MinutePrice);

        await _rentRepository.AddAsync(startedRent.Value);
        await _dbContext.SaveChangesAsync();
        return startedRent.Value.Id;
    }

    public async Task<Result<long>> EndRentAsync(long rentId, EndRentRequest request)
    {
        if (await _currentUserContext.GetUserAsync() is not {} account)
            return new NotFoundAccountError();
        
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if (rent is null)
            return new NotFoundRentError();

        if (account.Id != rent.UserId)
            return new NoAccessToRentError();

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
        
        //Pay for rent
        if (endedRent.FinalPrice is not null)
        {
            var payResult = account.Pay(endedRent.FinalPrice.Value);
            if (payResult.IsFailed) return Result.Fail(payResult.Errors);
        }
        _accountRepository.Update(account);
        
        await _dbContext.SaveChangesAsync();
        return endedRent.Id;
    }
}