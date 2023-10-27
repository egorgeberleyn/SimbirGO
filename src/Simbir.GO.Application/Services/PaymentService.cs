using FluentResults;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;

namespace Simbir.GO.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IAppDbContext _dbContext;
    private readonly IAccountRepository _accountRepository;

    public PaymentService(IAccountRepository accountRepository, IAppDbContext dbContext)
    {
        _accountRepository = accountRepository;
        _dbContext = dbContext;
    }

    public async Task<Result<Success>> PaymentHesoyamAsync(long accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account is null)
            return new Error("");
        
        var addResult = account.AddBalance(250_000);

        _accountRepository.Update(account);
        await _dbContext.SaveChangesAsync();
        return addResult;
    }
}