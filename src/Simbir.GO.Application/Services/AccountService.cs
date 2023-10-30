using FluentResults;
using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Services.Common;
using Simbir.GO.Application.Specifications.Accounts;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Accounts.Errors;

namespace Simbir.GO.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAppDbContext _dbContext;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AccountService(IAppDbContext dbContext, IAccountRepository accountRepository, ICurrentUserContext currentUserContext, 
        IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _accountRepository = accountRepository;
        _currentUserContext = currentUserContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<Account>> GetCurrentAccountAsync()
    {
        var account = await _currentUserContext.GetUserAsync();
        if (account is null)
            return new NotFoundAccountError();
        return account;
    }

    public async Task<Result<AuthResult>> SignInAsync(SignInAccountRequest request)
    {
        var usernameSpec = new ByUsernameSpec(request.Username);
        var account = await _accountRepository.GetByAsync(usernameSpec);

        if (account is null)
            return new NotFoundAccountError();

        var isSuccess = _passwordHasher.VerifyPassword(request.Password, account.PasswordHash, account.PasswordSalt);
        if (!isSuccess)
            return new InvalidCredentialsError();

        var tokenPair = await _jwtTokenGenerator.GenerateTokenPairAsync(account);
        return new AuthResult(tokenPair.accessToken, tokenPair.refreshToken);
    }

    public async Task<Result<Success>> SignUpAsync(SignUpAccountRequest request)
    {
        var usernameSpec = new ByUsernameSpec(request.Username);
        if (await _accountRepository.GetByAsync(usernameSpec) is not null)
            return new AlreadyExistsAccountError(request.Username);
        
        var (hash, salt) = _passwordHasher.HashPassword(request.Password);
        var createdAccount = Account.Create(request.Username, hash, salt, balanceValue: 0, role: nameof(Role.Client));
        if (createdAccount.IsFailed)
            return Result.Fail(createdAccount.Errors);
        
        await _accountRepository.AddAsync(createdAccount.Value);
        await _dbContext.SaveChangesAsync();
        return new Success("Account created successfully");
    }

    public async Task<Result<Success>> UpdateAccountAsync(UpdateAccountRequest request)
    {
        var usernameSpec = new ByUsernameSpec(request.Username);
        if (await _accountRepository.GetByAsync(usernameSpec) is not null)
            return new AlreadyExistsAccountError(request.Username);
        
        var currentAccount = await _currentUserContext.GetUserAsync();
        if (currentAccount is null)
            return new NotFoundAccountError();
        
        var (hash, salt) = _passwordHasher.HashPassword(request.Password);
        var updatedAccount = currentAccount.Edit(request.Username, hash, salt);
        if(updatedAccount.IsFailed)
            return Result.Fail(updatedAccount.Errors);

        _accountRepository.Update(updatedAccount.Value);
        await _dbContext.SaveChangesAsync();
        return new Success($"Account [{updatedAccount.Value.Username}] successfully updated");
    }

    public Task SignOutAsync()
    {
        throw new NotImplementedException();
    }
}