using FluentResults;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Accounts.Errors;

namespace Simbir.GO.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly IAccountRepository _accountRepository;

    public PaymentService(IAccountRepository accountRepository, IAppDbContext dbContext, 
        IUserContext userContext)
    {
        _accountRepository = accountRepository;
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<Result<Success>> PaymentHesoyamAsync(long accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account is null)
            return new NotFoundAccountError();

        if (await _userContext.GetUserAsync() is not { } currentUser)
            return new NotFoundAccountError();

        if (currentUser.Role == Role.Client && accountId != currentUser.Id)
            return new NoRightsAccountError();
        
        var addResult = account.AddBalance(250_000);

        _accountRepository.Update(account);
        await _dbContext.SaveChangesAsync();
        return addResult;
    }
}