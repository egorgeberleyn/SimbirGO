using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Accounts;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Specifications.Accounts;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Accounts.Errors;

namespace Simbir.GO.Application.Services.Admin;

public class AdminAccountService
{
    private readonly IAppDbContext _dbContext;
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AdminAccountService(IAppDbContext dbContext, IAccountRepository accountRepository, 
        IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<List<Account>>> GetAccountsAsync(int start, int count)
    {
        var byCountFilterSpec = new ByCountFilterSpec(start, count);
        return await _accountRepository.GetAllByAsync(byCountFilterSpec);
    }

    public async Task<Result<Account>> GetAccountAsync(long id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        return account is null
            ? new NotFoundAccountError()
            : account;
    }

    public async Task<Result<Success>> CreateAccountAsync(CreateAccountRequest request)
    {
        var usernameSpec = new ByUsernameSpec(request.Username);
        if (await _accountRepository.GetByAsync(usernameSpec) is not null)
            return new AlreadyExistsAccountError(request.Username);
        
        var (hash, salt) = _passwordHasher.HashPassword(request.Password);

        var role = request.IsAdmin ? nameof(Role.Admin) : nameof(Role.Client);
        var createdAccount = Account.Create(request.Username, hash, salt, request.Balance, role);
        if (createdAccount.IsFailed)
            return Result.Fail(createdAccount.Errors);
        
        await _accountRepository.AddAsync(createdAccount.Value);
        await _dbContext.SaveChangesAsync();
        return new Success("Account created");
    }

    public async Task<Result<Success>> UpdateAccountAsync(long id, AdminUpdateAccountRequest request)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if(account is null)
            return new NotFoundAccountError();
        
        var usernameSpec = new ByUsernameSpec(request.Username);
        if (await _accountRepository.GetByAsync(usernameSpec) is not null)
            return new AlreadyExistsAccountError(request.Username);
        
        var (hash, salt) = _passwordHasher.HashPassword(request.Password);
        var role = request.IsAdmin ? nameof(Role.Admin) : nameof(Role.Client);
        var updatedAccount = account.Update(request.Username, hash, salt, request.Balance, role);
        if(updatedAccount.IsFailed)
            return Result.Fail(updatedAccount.Errors);

        _accountRepository.Update(updatedAccount.Value);
        await _dbContext.SaveChangesAsync();
        return new Success($"Account [{updatedAccount.Value.Username}] updated");
    }

    public async Task<Result<Success>> DeleteAccountAsync(long id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if(account is null)
            return new NotFoundAccountError();
        
        _accountRepository.Delete(account);
        await _dbContext.SaveChangesAsync();
        return new Success($"Account [{account.Username}] deleted");
    }
}