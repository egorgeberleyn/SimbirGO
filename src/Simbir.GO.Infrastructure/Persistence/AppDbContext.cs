using Microsoft.EntityFrameworkCore;
using Simbir.GO.Application.Interfaces.Persistence;
using Simbir.GO.Application.Services.Common;
using Simbir.GO.Domain.Accounts;
using Simbir.GO.Domain.Rents;
using Simbir.GO.Domain.Transports;

namespace Simbir.GO.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Rent> Rents { get; set; } = null!;
    public DbSet<Transport> Transports { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}