using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Rents;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Specifications.Rents;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Domain.Rents;

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
            ? new Error("")
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
            return new Error("");

        var byTransportSpec = new ByTransportSpec(transportId);
        var transportRents = await _rentRepository.GetAllByAsync(byTransportSpec);

        return transportRents;
    }

    public async Task<Result<long>> CreateRentAsync(CreateRentRequest request)
    {
        var transport = await _transportRepository.GetByIdAsync(request.TransportId);
        if (transport is null)
            return new Error("");
        
        var startedRent = Rent.Create(transport.Id, request.UserId, request.TimeStart, request.TimeEnd,
            request.PriceOfUnit, request.PriceType, request.FinalPrice);

        await _rentRepository.AddAsync(startedRent.Value);
        await _dbContext.SaveChangesAsync();
        return startedRent.Value.Id;
    }

    public async Task<Result<long>> AdminEndRentAsync(long rentId, AdminEndRentRequest request)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if (rent is null)
            return new Error("");

        var endedRent = rent.End();
        
        _rentRepository.Update(endedRent.Value);
        await _dbContext.SaveChangesAsync();
        return endedRent.Value.Id;
    }

    public async Task<Result<long>> UpdateRentAsync(long rentId, UpdateRentRequest request)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if (rent is null)
            return new Error("");

        var updatedRent = rent.Update(request.TransportId, request.UserId, request.TimeStart, request.TimeEnd,
            request.PriceOfUnit, request.PriceType, request.FinalPrice);
        
        _rentRepository.Update(updatedRent.Value);
        await _dbContext.SaveChangesAsync();
        return updatedRent.Value.Id;
    }

    public async Task<Result<long>> DeleteRentAsync(long rentId)
    {
        var rent = await _rentRepository.GetByIdAsync(rentId);
        if (rent is null)
            return new Error("");
        
        _rentRepository.Delete(rent);
        await _dbContext.SaveChangesAsync();
        return rent.Id;
    }
}