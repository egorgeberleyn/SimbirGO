using Simbir.GO.Application.Contracts.Accounts;
using Simbir.GO.Application.Interfaces;
using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Application.Services;

public class AccountService : IAccountService
{
    public Task<Account> GetCurrentAccountAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> SignInAsync(SignInAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<string> SignUpAsync(SignUpAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<long> UpdateAccountAsync(UpdateAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync()
    {
        throw new NotImplementedException();
    }
}