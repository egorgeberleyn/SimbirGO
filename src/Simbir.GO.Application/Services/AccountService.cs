using FluentResults;
using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Services.Common;
using Simbir.GO.Application.Specifications;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.Errors;

namespace Simbir.GO.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAppDbContext _dbContext;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AccountService(IAppDbContext dbContext, IAccountRepository accountRepository, IUserService userService, 
        IPasswordService passwordService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _accountRepository = accountRepository;
        _userService = userService;
        _passwordService = passwordService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<Account>> GetCurrentAccountAsync()
    {
        var account = await _userService.GetUserAsync();
        if (account is null)
            return new NotExistsAccountError();
        return account;
    }

    public async Task<Result<AuthResult>> SignInAsync(SignInAccountRequest request)
    {
        var usernameSpec = new ByUsernameSpec(request.Username);
        var account = await _accountRepository.GetBy(usernameSpec);

        if (account is null)
            return new NotExistsAccountError();

        var isSuccess = _passwordService.VerifyPassword(request.Password, account.PasswordHash, account.PasswordSalt);
        if (!isSuccess)
            return new Error("");

        var tokenPair = await _jwtTokenGenerator.GenerateTokenPairAsync(account);
        return new AuthResult(tokenPair.accessToken, tokenPair.refreshToken);
    }

    public async Task<Result<Success>> SignUpAsync(SignUpAccountRequest request)
    {
        var (hash, salt) = _passwordService.HashPassword(request.Password);
        var createdAccount = Account.Create(request.Username, hash, salt, balanceValue: 0, role: "Client");
        if (createdAccount.IsFailed)
            return Result.Fail(createdAccount.Errors[0]);
        
        await _accountRepository.AddAccountAsync(createdAccount.Value);
        await _dbContext.SaveChangesAsync();
        return new Success("Ok");
    }

    public async Task<Result<long>> UpdateAccountAsync(UpdateAccountRequest request)
    {
        var account = await _userService.GetUserAsync();
        if (account is null)
            return new NotExistsAccountError();
        
        var (hash, salt) = _passwordService.HashPassword(request.Password);
        var updatedAccount = account.Edit(request.Username, hash, salt);
        if(updatedAccount.IsFailed)
            return Result.Fail(updatedAccount.Errors[0]);

        _accountRepository.Update(updatedAccount.Value);
        await _dbContext.SaveChangesAsync();
        return updatedAccount.Value.Id;
    }

    public Task SignOutAsync()
    {
        throw new NotImplementedException();
    }
}