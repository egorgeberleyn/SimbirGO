using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Simbir.GO.Application.Interfaces.Auth;

namespace Simbir.GO.Infrastructure.Persistence;

public class DbContextAppInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DbContextAppInitializer> _logger;

    public DbContextAppInitializer(IServiceProvider serviceProvider, ILogger<DbContextAppInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        _logger.LogInformation("Running DB context: {Name}", appDbContext.GetType().Name);

        await appDbContext.Database.MigrateAsync(cancellationToken);
        await SeedData.SeedAsync(appDbContext, passwordHasher);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}