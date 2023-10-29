using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Accounts;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Specifications.Accounts;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Accounts.Errors;

namespace Simbir.GO.Application.Services.Admin;

public class AdminAccountService : IAdminAccountService
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

    public Task<Result<List<Account>>> GetAccountsAsync(int start, int count)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Account>> GetAccountAsync(long id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        return account is null
            ? new Error("")
            : account;
    }

    public async Task<Result<long>> CreateAccountAsync(CreateAccountRequest request)
    {
        var usernameSpec = new ByUsernameSpec(request.Username);
        if (await _accountRepository.GetByAsync(usernameSpec) is not null)
            return new Error("");
        
        var (hash, salt) = _passwordHasher.HashPassword(request.Password);

        var role = request.IsAdmin ? nameof(Role.Admin) : nameof(Role.Client);
        var createdAccount = Account.Create(request.Username, hash, salt, request.Balance, role);
        if (createdAccount.IsFailed)
            return Result.Fail(createdAccount.Errors[0]);
        
        await _accountRepository.AddAsync(createdAccount.Value);
        await _dbContext.SaveChangesAsync();
        return createdAccount.Value.Id;
    }

    public async Task<Result<long>> UpdateAccountAsync(long id, UpdateAccountRequest request)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if(account is null)
            return new NotExistsAccountError();
        
        var (hash, salt) = _passwordHasher.HashPassword(request.Password);
        var role = request.IsAdmin ? nameof(Role.Admin) : nameof(Role.Client);
        var updatedAccount = account.Update(request.Username, hash, salt, request.Balance, role);
        if(updatedAccount.IsFailed)
            return Result.Fail(updatedAccount.Errors[0]);

        _accountRepository.Update(updatedAccount.Value);
        await _dbContext.SaveChangesAsync();
        return updatedAccount.Value.Id;
    }

    public async Task<Result<long>> DeleteAccountAsync(long id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if(account is null)
            return new NotExistsAccountError();
        
        _accountRepository.Delete(account);
        await _dbContext.SaveChangesAsync();
        return account.Id;
    }
}