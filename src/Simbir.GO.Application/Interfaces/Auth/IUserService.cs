using Simbir.GO.Domain.Accounts;

namespace Simbir.GO.Application.Interfaces.Auth;

public interface IUserService
{
    bool TryGetUserId(out long result);
    Task<Account?> GetUserAsync();
}