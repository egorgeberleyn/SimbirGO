using Simbir.GO.Domain.Accounts.Enums;

namespace Simbir.GO.Application.Contracts.Accounts;

public record AccountResponse(
    string Username,
    BalanceResponse Balance,
    Role Role);

public record BalanceResponse(double Value);
