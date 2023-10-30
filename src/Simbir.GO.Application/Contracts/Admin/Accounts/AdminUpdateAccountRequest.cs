namespace Simbir.GO.Application.Contracts.Admin.Accounts;

public record AdminUpdateAccountRequest(
    string Username, 
    string Password,
    bool IsAdmin,
    double Balance);