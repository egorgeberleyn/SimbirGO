using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Shared.Entities;
using StatusGeneric;

namespace Simbir.GO.Domain.Accounts.ValueObjects;

public class Balance : ValueObject
{
    public double Value { get; init; }
    public Currency Currency { get; init; }

    private Balance(double value, Currency currency) =>
        (Value, Currency) = (value, currency);

    public static IStatusGeneric<Balance> Create(double value, string currency)
    {
        var status = new StatusGenericHandler<Balance>();
        
        if (!Enum.TryParse<Currency>(currency, true, out var balanceCurrency))
            throw new Exception();
        return status.SetResult(new Balance(value, balanceCurrency));
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
        yield return Currency;
    }
}