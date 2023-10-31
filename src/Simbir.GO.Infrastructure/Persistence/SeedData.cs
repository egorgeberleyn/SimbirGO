using Simbir.GO.Application.Interfaces.Auth;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Accounts.Enums;
using Simbir.GO.Domain.Transports;
using Simbir.GO.Domain.Transports.Enums;

namespace Simbir.GO.Infrastructure.Persistence;

public class SeedData
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        if (context.Accounts.Any()) return;

        var (adminHash, adminSalt) = passwordHasher.HashPassword("secret");
        var (clientHash, clientSalt) = passwordHasher.HashPassword("123456");

        var accounts = new List<Account>
        {
            Account.Create("Admin", adminHash, adminSalt, 500_000, nameof(Role.Admin)).Value,
            Account.Create("Client", clientHash, clientSalt, 0, nameof(Role.Client)).Value
        };

        var transports = new List<Transport>()
        {
            Transport.Create(1, true, nameof(TransportType.Car), "Tesla CyberTruck", "silver",
                "A777AA", null, 1000, 50_000, 75, 66).Value,
            Transport.Create(2, true, nameof(TransportType.Bike), "Naked", "blue",
                "M322ИИ", "Amazing", 550, 32_000, 80, -66).Value
        };

        await context.Accounts.AddRangeAsync(accounts);
        await context.Transports.AddRangeAsync(transports);
        await context.SaveChangesAsync();
    } 
}