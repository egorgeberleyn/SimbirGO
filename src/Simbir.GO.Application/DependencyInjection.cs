using Microsoft.Extensions.DependencyInjection;
using Simbir.GO.Application.Services;
using Simbir.GO.Application.Services.Admin;
using Simbir.GO.Domain.Transports.Services;

namespace Simbir.GO.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<LocationFinder>();
        
        services.AddScoped<AccountService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<RentService>();
        services.AddScoped<TransportService>();

        services.AddScoped<AdminAccountService>();
        services.AddScoped<AdminTransportService>();
        services.AddScoped<AdminRentService>();
        
        return services;
    }
}