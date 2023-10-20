using Microsoft.EntityFrameworkCore;

namespace Simbir.GO.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}