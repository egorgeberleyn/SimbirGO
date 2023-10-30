using FluentResults;
using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Application.Services.Common;
using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Application.Interfaces;

public interface IAccountService
{
    Task<Result<Account>> GetCurrentAccountAsync();
    Task<Result<AuthResult>> SignInAsync(SignInAccountRequest request);
    Task<Result<Success>> SignUpAsync(SignUpAccountRequest request);
    Task<Result<Success>> UpdateAccountAsync(UpdateAccountRequest request);
    Task SignOutAsync();
}