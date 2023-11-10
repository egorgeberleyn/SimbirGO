using FluentResults;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Domain.Accounts.ValueObjects;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Accounts;

public class Account : Entity
{
    public string Username { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public Balance Balance { get; private set; }
    public Role Role { get; private set; }

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
        var roleResult = Validate(role);
        if (roleResult.IsFailed)
            return Result.Fail(roleResult.Errors);
        
        var createdBalance = Balance.Create(balanceValue);
        return createdBalance.IsSuccess
            ? new Account(username, passwordHash, passwordSalt, createdBalance.Value, roleResult.Value)
            : Result.Fail(createdBalance.Errors);
    }
    
    public Result<Account> Update(string username, byte[] passwordHash, byte[] passwordSalt, 
        double balanceValue, string role)
    {
        var (_, isFailed, accountRole, errors) = Validate(role);
        if (isFailed)
            return Result.Fail(errors);
        
        var createdBalance = Balance.Create(balanceValue);
        if(createdBalance.IsFailed)
            return Result.Fail(createdBalance.Errors);

        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Balance = createdBalance.Value;
        Role = accountRole;

        return this;
    }
    
    public Result<Account> Edit(string username, byte[] passwordHash, byte[] passwordSalt)
    {
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        return this;
    }

    public Result<Success> AddBalance(double balanceValue)
    {
        var createdBalance = Balance.Create(Balance.Value + balanceValue);
        if(createdBalance.IsFailed)
            return Result.Fail(createdBalance.Errors);
        
        Balance = createdBalance.Value;
        return new Success("Balance successfully replenished");
    }

    public Result<Success> Pay(double price)
    {
        if (price > Balance.Value)
            return new NotEnoughMoneyError();

        var newBalance = Balance.Create(Balance.Value - price);
        if (newBalance.IsFailed)
            return Result.Fail(newBalance.Errors);

        Balance = newBalance.Value;
        return new Success("Successful payment");
    }
    
    private static Result<Role> Validate(string role)
    {
        return !Enum.TryParse<Role>(role, true, out var accountRole)
            ? Result.Fail(new IncorrectRoleError()) 
            : Result.Ok(accountRole);
    }

#pragma warning disable CS8618
    private Account() {}
}