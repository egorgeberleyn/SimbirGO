using FluentResults;
using Simbir.GO.Application.Contracts.Admin.Accounts;
using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Application.Interfaces;

public interface IAdminAccountService
{
    Task<Result<List<Account>>> GetAccountsAsync(int start, int count);
    Task<Result<Account>> GetAccountAsync(long id);

    Task<Result<long>> CreateAccountAsync(CreateAccountRequest request);
    Task<Result<long>> UpdateAccountAsync(long id, UpdateAccountRequest request);
    Task<Result<long>> DeleteAccountAsync(long id);
}