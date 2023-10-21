using Simbir.GO.Domain.Accounts.ValueObjects;
using Simbir.GO.Shared.Entities;
using StatusGeneric;

namespace Simbir.GO.Domain.Accounts;

public class Account : Entity
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public Balance Balance { get; private set; }
    
    private Account(string username, string passwordHash, Balance balance)
    {
        Username = username;
        PasswordHash = passwordHash;
        Balance = balance;
    }

    public static IStatusGeneric<Account> Create(string username, string passwordHash, string currency, double balanceValue)
    {
        var status = new StatusGenericHandler<Account>();
        
        var createdBalance = Balance.Create(balanceValue, currency);
        return status.CombineStatuses(createdBalance).HasErrors 
            ? status 
            : status.SetResult(new Account(username, passwordHash, createdBalance.Result));
    }
}