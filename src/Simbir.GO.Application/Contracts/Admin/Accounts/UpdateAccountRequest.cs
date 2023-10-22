namespace Simbir.GO.Application.Contracts.Admin.Accounts;

public record UpdateAccountRequest(
    string Username, 
    string Password,
    bool IsAdmin,
    double Balance);