using Microsoft.Extensions.DependencyInjection;

namespace Simbir.GO.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}