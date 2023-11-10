namespace Simbir.GO.Application.Contracts.Admin.Accounts;

public record CreateAccountRequest(
    string Username, 
    string Password,
    bool IsAdmin,
    double Balance);