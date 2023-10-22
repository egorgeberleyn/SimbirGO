namespace Simbir.GO.Application.Contracts.Accounts;

public record AccountResponse(
    string Username, 
    string AccountCurrency, 
    double AccountBalance);
