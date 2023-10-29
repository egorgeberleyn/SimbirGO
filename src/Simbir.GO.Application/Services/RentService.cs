using FluentResults;
using Simbir.GO.Application.Contracts.Rents;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Specifications.Rents;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Domain.Rents;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Application.Services;

public class RentService : IRentService
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly IRentRepository _rentRepository;
    private readonly ITransportRepository _transportRepository;

    public RentService(IRentRepository rentRepository, IUserContext userContext, 
        ITransportRepository transportRepository, IAppDbContext dbContext)
    {
        _rentRepository = rentRepository;
        _userContext = userContext;
        _transportRepository = transportRepository;
        _dbContext = dbContext;
    }

    public Task<Result<Transport>> GetRentalTransportAsync(GetRentalTransportRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Rent>> GetRentByIdAsync(long rentId)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        return rent is null 
            ? new Error("")
            : rent;
    }

    public async Task<Result<List<Rent>>> GetMyRentHistoryAsync()
    {
        var currentAccount = await _userContext.GetUserAsync();
        if (currentAccount is null)
            return new NotExistsAccountError();

        var byAccountSpec = new ByAccountSpec(currentAccount.Id);
        var accountRents = await _rentRepository.GetAllByAsync(byAccountSpec);

        return accountRents;
    }

    public async Task<Result<List<Rent>>> GetTransportRentHistoryAsync(long transportId)
    {
        var transport = await _transportRepository.GetByIdAsync(transportId);
        if (transport is null)
            return new Error("");

        var byTransportSpec = new ByTransportSpec(transportId);
        var transportRents = await _rentRepository.GetAllByAsync(byTransportSpec);

        return transportRents;
    }

    public async Task<Result<long>> StartRentAsync(long transportId, StartRentRequest request)
    {
        if (!_userContext.TryGetUserId(out var accountId))
            return new NotExistsAccountError();
        
        var transport = await _transportRepository.GetByIdAsync(transportId);
        if (transport is null)
            return new Error("");
        
        var startedRent = Rent.Start(transportId, accountId, 0, request.RentType);

        await _rentRepository.AddAsync(startedRent.Value);
        await _dbContext.SaveChangesAsync();
        return startedRent.Value.Id;
    }

    public async Task<Result<long>> EndRentAsync(long rentId, EndRentRequest request)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if (rent is null)
            return new Error("");

        var endedRent = rent.End();
        
        _rentRepository.Update(endedRent.Value);
        await _dbContext.SaveChangesAsync();
        return endedRent.Value.Id;
    }
}