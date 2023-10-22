using FluentResults;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Accounts.ValueObjects;

public class Balance : ValueObject
{
    public double Value { get; init; }
    
    private Balance(double value) =>
        Value = value;

    public static Result<Balance> Create(double value)
    {
        if(value < 0)
            return new BelowZeroBalanceError(value);
        
        return new Balance(value);
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}