using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Application.Interfaces;

public interface IAccountService
{
    Task<Account> GetCurrentAccountAsync();
    Task<string> SignInAsync(SignInAccountRequest request);
    Task<string> SignUpAsync(SignUpAccountRequest request);
    Task<long> UpdateAccountAsync(UpdateAccountRequest request);
    Task SignOutAsync();
}