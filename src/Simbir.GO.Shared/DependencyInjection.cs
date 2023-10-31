using Microsoft.Extensions.DependencyInjection;
using Simbir.GO.Shared.Persistence.Repositories;

namespace Simbir.GO.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}