using Simbir.GO.Domain.Accounts;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Application.Interfaces.Persistence.Repositories;

public interface IAccountRepository
{
    Task<Account?> FindAccountByIdAsync(long accountId);
    Task<Account?> GetBy(Specification<Account> spec);
    Task<long> AddAccountAsync(Account account);
    long Update(Account updatedAccount);
}