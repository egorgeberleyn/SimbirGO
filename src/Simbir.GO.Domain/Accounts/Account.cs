using FluentResults;
using Simbir.GO.Domain.Accounts.Entities;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Accounts.ValueObjects;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Accounts;

public class Account : Entity
{
    private readonly List<AccountRent> _accountRents = new();
    
    public string Username { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public Balance Balance { get; private set; }
    public Role Role { get; private set; }
    public bool IsAdmin { get; private set; }
    public IReadOnlyList<AccountRent> AccountRents => _accountRents;

    private Account(string username, byte[] passwordHash, byte[] passwordSalt, Balance balance, Role role)
    {
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Balance = balance;
        Role = role;
    }

    public static Result<Account> Create(string username, byte[] passwordHash, byte[] passwordSalt, 
        double balanceValue, string role)
    {
        var roleResult = ValidateRole(role);
        if (roleResult.IsFailed)
            return Result.Fail(roleResult.Errors[0]);
        
        var createdBalance = Balance.Create(balanceValue);
        return createdBalance.IsSuccess
            ? new Account(username, passwordHash, passwordSalt, createdBalance.Value, roleResult.Value)
            : Result.Fail(createdBalance.Errors[0]);
    }
    
    public Result<Account> Update(string username, byte[] passwordHash, byte[] passwordSalt, 
        double balanceValue, string role)
    {
        var roleResult = ValidateRole(role);
        if (roleResult.IsFailed)
            return Result.Fail(roleResult.Errors[0]);
        
        var createdBalance = Balance.Create(balanceValue);
        if(createdBalance.IsFailed)
            return Result.Fail(createdBalance.Errors[0]);

        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Balance = createdBalance.Value;
        Role = roleResult.Value;

        return this;
    }
    
    public Result<Account> Edit(string username, byte[] passwordHash, byte[] passwordSalt)
    {
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        return this;
    }
    
    private static Result<Role> ValidateRole(string role)
    {
        return !Enum.TryParse<Role>(role, true, out var accountRole)
            ? Result.Fail(new Error("type")) 
            : Result.Ok(accountRole);
    } 
}