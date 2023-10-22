using FluentResults;
using Simbir.GO.Domain.Accounts.ValueObjects;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Accounts;

public class Account : Entity
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public Balance Balance { get; private set; }
    public bool IsAdmin { get; private set; }

    private Account(string username, string passwordHash, Balance balance)
    {
        Username = username;
        PasswordHash = passwordHash;
        Balance = balance;
    }

    public static Result<Account> Create(string username, string passwordHash, string currency, double balanceValue)
    {
        var createdBalance = Balance.Create(balanceValue);
        return createdBalance.IsSuccess
            ? new Account(username, passwordHash, createdBalance.Value)
            : Result.Fail(createdBalance.Errors[0]);
    }
}