using FluentResults;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Accounts.Errors;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Domain.Accounts.ValueObjects;

public class Balance : ValueObject
{
    public double Value { get; init; }
    public Currency Currency { get; init; }

    private Balance(double value, Currency currency) =>
        (Value, Currency) = (value, currency);

    public static Result<Balance> Create(double value, string currency)
    {
        if (!Enum.TryParse<Currency>(currency, true, out var balanceCurrency))
            return new IncorrectCurrencyFormatError(currency);
        
        return new Balance(value, balanceCurrency);
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
        yield return Currency;
    }
}