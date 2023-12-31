﻿using FluentResults;
using Microsoft.AspNetCore.Http;
using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Interfaces.Persistence.Repositories;
using Simbir.GO.Application.Services.Common;
using Simbir.GO.Application.Specifications.Accounts;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Accounts.Errors;

namespace Simbir.GO.Application.Services;

public class AccountService
{
    private readonly IAppDbContext _dbContext;
    private readonly IAccountRepository _accountRepository;
    private readonly IRevokedTokenRepository _revokedTokenRepository;
    private readonly IUserContext _userContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IHttpContextAccessor _contextAccessor;

    public AccountService(IAppDbContext dbContext, IAccountRepository accountRepository, IUserContext userContext, 
        IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IHttpContextAccessor contextAccessor, 
        IRevokedTokenRepository revokedTokenRepository)
    {
        _dbContext = dbContext;
        _accountRepository = accountRepository;
        _userContext = userContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _contextAccessor = contextAccessor;
        _revokedTokenRepository = revokedTokenRepository;
    }

    public async Task<Result<Account>> GetCurrentAccountAsync()
    {
        var account = await _userContext.GetUserAsync();
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

        var accessToken = _jwtTokenGenerator.GenerateToken(account);
        return new AuthResult(accessToken);
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
        return new Success("Account created");
    }

    public async Task<Result<Success>> UpdateAccountAsync(UpdateAccountRequest request)
    {
        var usernameSpec = new ByUsernameSpec(request.Username);
        if (await _accountRepository.GetByAsync(usernameSpec) is not null)
            return new AlreadyExistsAccountError(request.Username);
        
        var currentAccount = await _userContext.GetUserAsync();
        if (currentAccount is null)
            return new NotFoundAccountError();
        
        var (hash, salt) = _passwordHasher.HashPassword(request.Password);
        var updatedAccount = currentAccount.Edit(request.Username, hash, salt);
        if(updatedAccount.IsFailed)
            return Result.Fail(updatedAccount.Errors);

        _accountRepository.Update(updatedAccount.Value);
        await _dbContext.SaveChangesAsync();
        return new Success($"Account [{updatedAccount.Value.Username}] updated");
    }

    public async Task<Result<Success>> SignOutAsync()
    {
        var tokenStr = _contextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString()
            .Replace("bearer ", string.Empty);

        if (tokenStr is null)
            return new UnauthorizedError();
        
        var jwtToken = _jwtTokenGenerator.ParseToken(tokenStr);
        
        if(!_userContext.TryGetUserId(out var userId))
            return Result.Fail(new NotFoundAccountError());

        var revokedToken = new RevokedToken()
        {
            Token = tokenStr,
            IsRevoked = true,
            AddedDate = DateTime.UtcNow,
            UserId = userId,
            JwtId = jwtToken.Id
        };

        await _revokedTokenRepository.AddAsync(revokedToken);
        await _dbContext.SaveChangesAsync();
        return new Success("Sign out successfully");
    }
}